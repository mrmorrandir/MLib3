using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FluentResults;
using Microsoft.Extensions.Options;

namespace MLib3.Logging;

/// <summary>
///     Default implementation of the logging payload sanitizer.
/// </summary>
public sealed class LogPayloadSanitizer : ILogPayloadSanitizer
{
    private const string CircularReferenceReplacement = "[CIRCULAR]";
    private readonly int _maxDepth;

    /// <summary>
    ///     The ConditionalWeakTable allows the garbage collector to free no longer used types and
    ///     therefore allows dynamic types (the ones that might be used as key <c>Type</c>) to vanish
    ///     from the table instead of holding on them forever (and using up all memory).
    ///     It's like a <c>Dictionary</c> only smarter and thread-safe.
    /// </summary>
    private readonly ConditionalWeakTable<Type, TypeMetadata> _metadataCache = new();

    private readonly HashSet<string> _sensitivePropertyNames;
    private readonly JsonSerializerOptions _serializerOptions;

    /// <summary>
    ///     Initializes a sanitizer with the configured serializer, sensitive property names,
    ///     and traversal depth.
    /// </summary>
    /// <param name="options">The options that control payload sanitization.</param>
    public LogPayloadSanitizer(IOptions<LogPayloadSanitizerOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _serializerOptions = new JsonSerializerOptions(options.Value.SerializerOptions);
        _sensitivePropertyNames = new HashSet<string>(
            options.Value.SensitivePropertyNames,
            StringComparer.OrdinalIgnoreCase);
        _maxDepth = options.Value.MaxDepth;
    }

    /// <summary>
    ///     Creates a sanitized JSON representation of a typed logging payload without
    ///     modifying the source object.
    /// </summary>
    /// <param name="payload">The source payload to sanitize.</param>
    /// <param name="declaredType">
    ///     The declared payload type used to resolve logging attributes. When omitted,
    ///     the runtime type is used.
    /// </param>
    /// <param name="serializerOptions">
    ///     Optional serializer settings used for property names and scalar values.
    /// </param>
    /// <returns>
    ///     A successful sanitized representation, an excluded result for ignored root
    ///     types, or a failed result with no payload when sanitization cannot be completed.
    /// </returns>
    public LogPayloadSanitizationResult Sanitize(
        object? payload,
        Type? declaredType = null,
        JsonSerializerOptions? serializerOptions = null)
    {
        try
        {
            var options = serializerOptions ?? _serializerOptions;
            var payloadType = declaredType ?? payload?.GetType();
            if (payloadType is not null && GetMetadata(payloadType).IsIgnored)
                return LogPayloadSanitizationResult.Excluded();

            var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
            var node = BuildNode(payload, payloadType, options, visited, 0);
            var json = node?.ToJsonString(options) ?? "null";
            return LogPayloadSanitizationResult.Success(json);
        }
        catch
        {
            return LogPayloadSanitizationResult.Failed();
        }
    }

    /// <summary>
    ///     Sanitizes an existing JSON payload by applying type attributes and configured
    ///     sensitive property names.
    /// </summary>
    /// <param name="payload">The JSON payload to sanitize.</param>
    /// <param name="declaredType">
    ///     The declared DTO type used to associate JSON properties with attributed
    ///     properties. A missing type limits processing to name-based sanitization.
    /// </param>
    /// <param name="serializerOptions">
    ///     Optional serializer settings used to resolve configured JSON property names.
    /// </param>
    /// <returns>
    ///     A successful sanitized representation, an excluded result for ignored root
    ///     types, or a failed result with no payload when parsing or sanitization fails.
    /// </returns>
    public LogPayloadSanitizationResult SanitizeJson(
        string payload,
        Type? declaredType = null,
        JsonSerializerOptions? serializerOptions = null)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return LogPayloadSanitizationResult.Success(payload);

        try
        {
            var options = serializerOptions ?? _serializerOptions;
            if (declaredType is not null && GetMetadata(declaredType).IsIgnored)
                return LogPayloadSanitizationResult.Excluded();

            var node = JsonNode.Parse(payload);
            SanitizeJsonNode(node, declaredType, options, 0);
            return LogPayloadSanitizationResult.Success(node?.ToJsonString(options) ?? "null");
        }
        catch
        {
            return LogPayloadSanitizationResult.Failed();
        }
    }

    private sealed record TypeMetadata(bool IsIgnored, IReadOnlyList<PropertyMetadata> Properties);

    private sealed record PropertyMetadata(
        PropertyInfo Property,
        bool IsIgnored,
        string? Replacement,
        string? ExplicitJsonName);

    /// <summary>
    ///     Recursively converts an object value into a sanitized JSON node.
    /// </summary>
    /// <param name="value">The current value being converted.</param>
    /// <param name="declaredType">The declared type of the current value.</param>
    /// <param name="options">The serializer settings used by the conversion.</param>
    /// <param name="visited">The active reference path used to detect cycles.</param>
    /// <param name="depth">The current traversal depth.</param>
    /// <returns>
    ///     The sanitized JSON node, or <see langword="null" /> for null or ignored values.
    /// </returns>
    private JsonNode? BuildNode(
        object? value,
        Type? declaredType,
        JsonSerializerOptions options,
        HashSet<object> visited,
        int depth)
    {
        EnsureDepth(depth);

        if (value is null)
            return null;

        var runtimeType = value.GetType();
        if (GetMetadata(runtimeType).IsIgnored
            || (declaredType is not null && GetMetadata(UnwrapNullable(declaredType)).IsIgnored))
            return null;

        if (IsScalar(runtimeType))
            return JsonSerializer.SerializeToNode(value, runtimeType, options);

        if (!runtimeType.IsValueType && !visited.Add(value))
            return JsonValue.Create(CircularReferenceReplacement);

        try
        {
            if (value is IResultBase result)
                return BuildResultNode(result, runtimeType, options, visited, depth);

            if (TryGetDictionaryTypes(runtimeType, out _, out var dictionaryValueType))
                return BuildDictionaryNode(value, dictionaryValueType, options, visited, depth);

            if (value is IEnumerable enumerable)
                return BuildCollectionNode(enumerable, GetElementType(runtimeType), options, visited, depth);

            return BuildObjectNode(value, runtimeType, options, visited, depth);
        }
        finally
        {
            if (!runtimeType.IsValueType)
                visited.Remove(value);
        }
    }

    /// <summary>
    ///     Builds a safe representation of a FluentResults result without accessing its
    ///     value when the result has failed.
    /// </summary>
    /// <param name="result">The FluentResults result to represent.</param>
    /// <param name="runtimeType">The concrete result type.</param>
    /// <param name="options">The serializer settings used for property names.</param>
    /// <param name="visited">The active reference path used to detect cycles.</param>
    /// <param name="depth">The current traversal depth.</param>
    /// <returns>A sanitized JSON object containing result status and safe result data.</returns>
    private JsonObject BuildResultNode(
        IResultBase result,
        Type runtimeType,
        JsonSerializerOptions options,
        HashSet<object> visited,
        int depth)
    {
        var node = new JsonObject
        {
            [GetJsonName(nameof(IResultBase.IsSuccess), options)] = result.IsSuccess,
            [GetJsonName(nameof(IResultBase.IsFailed), options)] = result.IsFailed,
            [GetJsonName(nameof(IResultBase.Errors), options)] =
                BuildNode(result.Errors, result.Errors.GetType(), options, visited, depth + 1),
            [GetJsonName(nameof(IResultBase.Successes), options)] =
                BuildNode(result.Successes, result.Successes.GetType(), options, visited, depth + 1)
        };

        if (result.IsSuccess)
        {
            var valueProperty = runtimeType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            if (valueProperty is not null)
                node[GetJsonName(valueProperty, options)] = BuildNode(
                    valueProperty.GetValue(result),
                    valueProperty.PropertyType,
                    options,
                    visited,
                    depth + 1);
        }

        return node;
    }

    /// <summary>
    ///     Builds a sanitized JSON object from a dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary to convert.</param>
    /// <param name="valueType">The declared dictionary value type, when available.</param>
    /// <param name="options">The serializer settings used for dictionary keys.</param>
    /// <param name="visited">The active reference path used to detect cycles.</param>
    /// <param name="depth">The current traversal depth.</param>
    /// <returns>A sanitized JSON object containing the dictionary entries.</returns>
    private JsonObject BuildDictionaryNode(
        object dictionary,
        Type? valueType,
        JsonSerializerOptions options,
        HashSet<object> visited,
        int depth)
    {
        var node = new JsonObject();
        foreach (var entry in (IEnumerable)dictionary)
        {
            var entryType = entry.GetType();
            var key = entryType.GetProperty("Key")?.GetValue(entry)?.ToString();
            var entryValue = entryType.GetProperty("Value")?.GetValue(entry);
            if (key is null)
                throw new JsonException("A logging dictionary key could not be represented as text.");

            var jsonKey = options.DictionaryKeyPolicy?.ConvertName(key) ?? key;
            node[jsonKey] = IsSensitiveName(key) || IsSensitiveName(jsonKey)
                ? JsonValue.Create(LogRedactAttribute.DefaultReplacement)
                : BuildNode(entryValue, valueType, options, visited, depth + 1);
        }

        return node;
    }

    /// <summary>
    ///     Builds a sanitized JSON array from an enumerable value.
    /// </summary>
    /// <param name="values">The values to convert.</param>
    /// <param name="elementType">The declared collection element type, when available.</param>
    /// <param name="options">The serializer settings used by nested values.</param>
    /// <param name="visited">The active reference path used to detect cycles.</param>
    /// <param name="depth">The current traversal depth.</param>
    /// <returns>A sanitized JSON array that excludes ignored element types.</returns>
    private JsonArray BuildCollectionNode(
        IEnumerable values,
        Type? elementType,
        JsonSerializerOptions options,
        HashSet<object> visited,
        int depth)
    {
        var node = new JsonArray();
        foreach (var item in values)
        {
            var itemType = item?.GetType() ?? elementType;
            if (itemType is not null && GetMetadata(UnwrapNullable(itemType)).IsIgnored)
                continue;

            node.Add(BuildNode(item, elementType, options, visited, depth + 1));
        }

        return node;
    }

    /// <summary>
    ///     Builds a sanitized JSON object from the readable public properties of an object.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="runtimeType">The concrete object type.</param>
    /// <param name="options">The serializer settings used for property names.</param>
    /// <param name="visited">The active reference path used to detect cycles.</param>
    /// <param name="depth">The current traversal depth.</param>
    /// <returns>A JSON object with ignored properties removed and sensitive values replaced.</returns>
    private JsonObject BuildObjectNode(
        object value,
        Type runtimeType,
        JsonSerializerOptions options,
        HashSet<object> visited,
        int depth)
    {
        var node = new JsonObject();
        foreach (var property in GetMetadata(runtimeType).Properties)
        {
            if (property.IsIgnored)
                continue;

            var jsonName = GetJsonName(property, options);
            if (property.Replacement is not null || IsSensitiveName(property.Property.Name) || IsSensitiveName(jsonName))
            {
                node[jsonName] = property.Replacement ?? LogRedactAttribute.DefaultReplacement;
                continue;
            }

            var propertyValue = property.Property.GetValue(value);
            if (GetMetadata(UnwrapNullable(property.Property.PropertyType)).IsIgnored)
                continue;

            node[jsonName] = BuildNode(
                propertyValue,
                property.Property.PropertyType,
                options,
                visited,
                depth + 1);
        }

        return node;
    }

    /// <summary>
    ///     Recursively sanitizes a parsed JSON node using the optional declared type.
    /// </summary>
    /// <param name="node">The JSON node to sanitize in the isolated logging representation.</param>
    /// <param name="declaredType">The declared type associated with the node, when known.</param>
    /// <param name="options">The serializer settings used to match JSON property names.</param>
    /// <param name="depth">The current traversal depth.</param>
    private void SanitizeJsonNode(
        JsonNode? node,
        Type? declaredType,
        JsonSerializerOptions options,
        int depth)
    {
        EnsureDepth(depth);

        if (node is JsonObject jsonObject)
        {
            // Check if the node is a dictionary and sanitize it when it is (because Dictionaries might have 
            // data to sanitize but do not have properties)
            if (declaredType is not null && TryGetDictionaryTypes(declaredType, out _, out var valueType))
            {
                SanitizeDictionaryJson(jsonObject, valueType, options, depth);
                return;
            }

            // Sanitize other types
            var metadata = declaredType is null ? null : GetMetadata(UnwrapNullable(declaredType));
            foreach (var entry in jsonObject.ToList())
            {
                var property = metadata?.Properties.FirstOrDefault(candidate =>
                    string.Equals(GetJsonName(candidate, options), entry.Key, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(candidate.Property.Name, entry.Key, StringComparison.OrdinalIgnoreCase));

                if (property?.IsIgnored == true
                    || (property is not null && GetMetadata(UnwrapNullable(property.Property.PropertyType)).IsIgnored))
                {
                    jsonObject.Remove(entry.Key);
                    continue;
                }

                if (property?.Replacement is not null
                    || IsSensitiveName(entry.Key)
                    || (property is not null && IsSensitiveName(property.Property.Name)))
                {
                    jsonObject[entry.Key] = property?.Replacement ?? LogRedactAttribute.DefaultReplacement;
                    continue;
                }

                SanitizeJsonNode(entry.Value, property?.Property.PropertyType, options, depth + 1);
            }

            return;
        }

        if (node is not JsonArray jsonArray)
            return;

        var elementType = declaredType is null ? null : GetElementType(declaredType);
        for (var index = jsonArray.Count - 1; index >= 0; index--)
        {
            if (elementType is not null && GetMetadata(UnwrapNullable(elementType)).IsIgnored)
            {
                jsonArray.RemoveAt(index);
                continue;
            }

            SanitizeJsonNode(jsonArray[index], elementType, options, depth + 1);
        }
    }

    /// <summary>
    ///     Sanitizes the entries of a parsed JSON dictionary.
    /// </summary>
    /// <param name="jsonObject">The isolated JSON dictionary representation.</param>
    /// <param name="valueType">The declared dictionary value type, when available.</param>
    /// <param name="options">The serializer settings used by nested values.</param>
    /// <param name="depth">The current traversal depth.</param>
    private void SanitizeDictionaryJson(
        JsonObject jsonObject,
        Type? valueType,
        JsonSerializerOptions options,
        int depth)
    {
        foreach (var entry in jsonObject.ToList())
        {
            if (IsSensitiveName(entry.Key))
            {
                jsonObject[entry.Key] = LogRedactAttribute.DefaultReplacement;
                continue;
            }

            SanitizeJsonNode(entry.Value, valueType, options, depth + 1);
        }
    }

    /// <summary>
    ///     Gets cached logging metadata for a type, creating it on first use.
    /// </summary>
    /// <param name="type">The type whose logging metadata is required.</param>
    /// <returns>The cached type metadata.</returns>
    private TypeMetadata GetMetadata(Type type) => _metadataCache.GetValue(type, CreateMetadata);

    /// <summary>
    ///     Discovers logging and JSON naming attributes for a type and its readable
    ///     public properties.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns>The immutable metadata used during sanitization.</returns>
    private static TypeMetadata CreateMetadata(Type type)
    {
        var properties = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetMethod is not null
                               && property.GetMethod.IsPublic
                               && property.GetIndexParameters().Length == 0)
            .Select(property => new PropertyMetadata(
                property,
                property.GetCustomAttribute<LogPayloadIgnoreAttribute>() is not null,
                property.GetCustomAttribute<LogRedactAttribute>()?.Replacement,
                property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name))
            .ToArray();

        return new TypeMetadata(
            type.GetCustomAttribute<LogPayloadIgnoreAttribute>() is not null,
            properties);
    }

    /// <summary>
    ///     Determines whether a property or dictionary key is included in the configured
    ///     defense-in-depth list.
    /// </summary>
    /// <param name="name">The CLR property name, JSON property name, or dictionary key.</param>
    /// <returns>
    ///     <see langword="true" /> when the name must be redacted; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    private bool IsSensitiveName(string name) => _sensitivePropertyNames.Contains(name);

    /// <summary>
    ///     Verifies that recursive traversal remains within the configured depth limit.
    /// </summary>
    /// <param name="depth">The current traversal depth.</param>
    /// <exception cref="JsonException">
    ///     Thrown when the configured maximum depth has been exceeded.
    /// </exception>
    private void EnsureDepth(int depth)
    {
        if (depth > _maxDepth)
            throw new JsonException("The logging payload exceeded the configured traversal depth.");
    }

    /// <summary>
    ///     Resolves the JSON name of a reflected property.
    /// </summary>
    /// <param name="property">The reflected property.</param>
    /// <param name="options">The serializer settings containing the naming policy.</param>
    /// <returns>The explicit or policy-derived JSON property name.</returns>
    private static string GetJsonName(PropertyInfo property, JsonSerializerOptions options)
    {
        var explicitName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
        return explicitName ?? GetJsonName(property.Name, options);
    }

    /// <summary>
    ///     Resolves the JSON name of a cached property metadata entry.
    /// </summary>
    /// <param name="property">The cached property metadata.</param>
    /// <param name="options">The serializer settings containing the naming policy.</param>
    /// <returns>The explicit or policy-derived JSON property name.</returns>
    private static string GetJsonName(PropertyMetadata property, JsonSerializerOptions options) => property.ExplicitJsonName ?? GetJsonName(property.Property.Name, options);

    /// <summary>
    ///     Applies the configured property naming policy to a CLR property name.
    /// </summary>
    /// <param name="clrName">The CLR property name.</param>
    /// <param name="options">The serializer settings containing the naming policy.</param>
    /// <returns>The policy-derived name, or the original name when no policy is configured.</returns>
    private static string GetJsonName(string clrName, JsonSerializerOptions options) => options.PropertyNamingPolicy?.ConvertName(clrName) ?? clrName;

    /// <summary>
    ///     Determines whether a type can be serialized as a terminal value without
    ///     recursive property traversal.
    /// </summary>
    /// <param name="type">The type to classify.</param>
    /// <returns>
    ///     <see langword="true" /> for supported scalar and JSON node types; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    private static bool IsScalar(Type type)
    {
        type = UnwrapNullable(type);
        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(DateOnly)
               || type == typeof(TimeOnly)
               || type == typeof(TimeSpan)
               || type == typeof(Guid)
               || type == typeof(Uri)
               || type == typeof(JsonElement)
               || typeof(JsonNode).IsAssignableFrom(type);
    }

    /// <summary>
    ///     Returns the underlying value type for a nullable type.
    /// </summary>
    /// <param name="type">The type to unwrap.</param>
    /// <returns>The underlying nullable type, or the original type when it is not nullable.</returns>
    private static Type UnwrapNullable(Type type) => Nullable.GetUnderlyingType(type) ?? type;

    /// <summary>
    ///     Resolves the element type exposed by an array or generic enumerable.
    /// </summary>
    /// <param name="type">The collection type to inspect.</param>
    /// <returns>The element type, or <see langword="null" /> when it cannot be determined.</returns>
    private static Type? GetElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();

        return type.GetInterfaces()
            .Append(type)
            .FirstOrDefault(candidate =>
                candidate.IsGenericType
                && candidate.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];
    }

    /// <summary>
    ///     Resolves the key and value types exposed by a generic dictionary contract.
    /// </summary>
    /// <param name="type">The candidate dictionary type.</param>
    /// <param name="keyType">The resolved key type when the method succeeds.</param>
    /// <param name="valueType">The resolved value type when the method succeeds.</param>
    /// <returns>
    ///     <see langword="true" /> when the type implements a supported dictionary contract;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    private static bool TryGetDictionaryTypes(Type type, out Type? keyType, out Type? valueType)
    {
        var dictionaryType = type.GetInterfaces()
            .Append(type)
            .FirstOrDefault(candidate =>
                candidate.IsGenericType
                && (candidate.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                    || candidate.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)));

        if (dictionaryType is null)
        {
            keyType = null;
            valueType = null;
            return false;
        }

        var arguments = dictionaryType.GetGenericArguments();
        keyType = arguments[0];
        valueType = arguments[1];
        return true;
    }
}
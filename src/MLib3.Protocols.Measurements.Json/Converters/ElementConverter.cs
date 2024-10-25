using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using FluentResults;
using MLib3.Protocols.Measurements.Serialization;

namespace MLib3.Protocols.Measurements.Json.Converters;

public class ElementConverter : JsonConverter<Element>
{
    public override Element Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var jsonRoot = jsonDoc.RootElement;
        var typeResult = Result.Try(() => jsonRoot.GetProperty("Type"));
        if (typeResult.IsFailed)
            throw new JsonException(typeResult.Errors.First().Message);
        
        var typeName = typeResult.Value.GetString();
        if (typeName is null)
            throw new JsonException("Type property is missing");

        if (!JsonTypeMapping.TypeNameToType.TryGetValue(typeName, out var type)) 
            throw new JsonException($"Unknown type: {typeName}");

        var deserializerTypeResult = Result.Try(() => JsonSerializer.Deserialize(jsonRoot.GetRawText(), type, options));
        if (deserializerTypeResult.IsFailed)
            throw new JsonException(deserializerTypeResult.Errors.First().Message);
        if (deserializerTypeResult.Value is null)
            throw new JsonException("Deserialized value is null");
        return (Element)deserializerTypeResult.Value;
    }

    public override void Write(Utf8JsonWriter writer, Element value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (!JsonTypeMapping.TypeToTypeName.TryGetValue(value.GetType(), out var typeName))
            throw new JsonException($"Unknown type: {value.GetType()}");

        writer.WriteString("Type", typeName);

        var properties = value.GetType().GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0); // Exclude indexer properties
        foreach (var property in properties)
        { 
            if (property.GetCustomAttribute<XmlIgnoreAttribute>() is not null)
                continue;
            var propertyValue = property.GetValue(value);
            if (propertyValue is null)
                continue;
            writer.WritePropertyName(property.Name);
            JsonSerializer.Serialize(writer, propertyValue, options);
        }
        writer.WriteEndObject();
    }
}
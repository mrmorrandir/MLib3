using System.Text.Json;

namespace MLib3.Logging;

/// <summary>
/// Creates non-mutating, safe logging representations of payloads.
/// </summary>
public interface ILogPayloadSanitizer
{
    LogPayloadSanitizationResult Sanitize(
        object? payload,
        Type? declaredType = null,
        JsonSerializerOptions? serializerOptions = null);

    LogPayloadSanitizationResult SanitizeJson(
        string payload,
        Type? declaredType = null,
        JsonSerializerOptions? serializerOptions = null);
}

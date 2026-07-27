using System.Text.Json;

namespace MLib3.Logging;

/// <summary>
/// Configures logging payload sanitization.
/// </summary>
public sealed class LogPayloadSanitizerOptions
{
    private static readonly string[] DefaultSensitivePropertyNames =
    [
        "password",
        "passwordHash",
        "token",
        "accessToken",
        "refreshToken",
        "secret",
        "clientSecret",
        "authorization",
        "cookie",
        "set-cookie"
    ];

    /// <summary>
    /// Gets or sets the serializer options used for typed logging payloads.
    /// </summary>
    public JsonSerializerOptions SerializerOptions { get; set; } = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Gets or sets case-insensitive property names that are always redacted.
    /// </summary>
    public ICollection<string> SensitivePropertyNames { get; set; } = new List<string>(DefaultSensitivePropertyNames);

    /// <summary>
    /// Gets or sets the maximum traversal depth.
    /// </summary>
    public int MaxDepth { get; set; } = 64;
}

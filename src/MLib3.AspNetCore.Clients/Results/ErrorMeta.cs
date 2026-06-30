namespace MLib3.AspNetCore.Clients;

/// <summary>
/// Represents a key-value pair for metadata associated with an error.
/// </summary>
public class ErrorMeta
{
    /// <summary>
    /// The key of the metadata entry.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The value of the metadata entry.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
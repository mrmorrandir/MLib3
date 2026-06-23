namespace MLib3.AspNetCore;

/// <summary>
/// Represents a key-value pair for metadata associated with an error.
/// </summary>
/// <remarks>
/// This class is used to provide additional contextual information related to an error.
/// Each instance contains a key, representing the metadata name, and a value, representing the metadata content.
/// </remarks>
public class ErrorMeta
{
    /// <summary>
    /// The key associated with the metadata in an error context.
    /// </summary>
    /// <remarks>
    /// This property represents the identifier or name of the metadata item.
    /// It is used to provide contextual information about an error, paired with the corresponding value in a key-value structure.
    /// </remarks>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The value associated with the metadata in an error context.
    /// </summary>
    /// <remarks>
    /// This property represents the content or data paired with a key in a key-value structure
    /// to provide additional contextual information about an error.
    /// </remarks>
    public string Value { get; set; } = string.Empty;
}
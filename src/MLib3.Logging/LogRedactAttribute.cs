namespace MLib3.Logging;

/// <summary>
/// Replaces a property value in logging payloads.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class LogRedactAttribute : Attribute
{
    /// <summary>
    /// The default value written in place of a sensitive value.
    /// </summary>
    public const string DefaultReplacement = "[REDACTED]";

    /// <summary>
    /// Initializes the attribute with the default replacement.
    /// </summary>
    public LogRedactAttribute()
        : this(DefaultReplacement)
    {
    }

    /// <summary>
    /// Initializes the attribute with a custom replacement.
    /// </summary>
    /// <param name="replacement">The value written to the logging payload.</param>
    public LogRedactAttribute(string replacement)
    {
        Replacement = replacement;
    }

    /// <summary>
    /// Gets the value written to the logging payload.
    /// </summary>
    public string Replacement { get; }
}

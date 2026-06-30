namespace MLib3.AspNetCore.Logging;

/// <summary>
/// Represents configuration options for logging specific to API requests.
/// </summary>
public class ApiLoggingOptions
{
    /// <summary>
    /// Gets or sets the collection of paths to be excluded from API logging.
    /// </summary>
    /// <remarks>
    /// When specified, any API requests matching the paths in this collection will be bypassed
    /// by the logging functionality. This can be useful for excluding sensitive or irrelevant
    /// endpoints from logging.
    /// </remarks>
    public string[]? ExcludedPaths { get; set; } = null;

    /// <summary>
    /// Gets or sets the collection of file names to be excluded from API logging.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When provided, any API interactions involving files matching the names in this collection
    /// will not be included in the logging process. This can be helpful for omitting sensitive
    /// or non-relevant file-related operations from being logged.
    /// </para>
    /// <para>Supports wildcards ('*') or regular expressions. (Wildcards will be handled via regular expressions, too)</para>
    /// </remarks>
    public string[]? ExcludedFiles { get; set; } = null;
}
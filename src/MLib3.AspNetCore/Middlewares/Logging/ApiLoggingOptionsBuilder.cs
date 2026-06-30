namespace MLib3.AspNetCore;

/// <summary>
/// A builder class for constructing and configuring instances of <see cref="ApiLoggingOptions"/>.
/// </summary>
/// <remarks>
/// This class provides methods to configure logging options for API requests,
/// such as specifying paths to be excluded from logging. Once configured,
/// the <see cref="Build"/> method returns a fully constructed and immutable instance
/// of <see cref="ApiLoggingOptions"/> ready for use in application middleware.
/// </remarks>
public class ApiLoggingOptionsBuilder
{
    private readonly ApiLoggingOptions _options = new();

    /// <summary>
    /// Configures the logging options to exclude the specified paths from being logged.
    /// </summary>
    /// <param name="excludedPaths">An array of paths to exclude from logging. These paths will not be logged by the API logging middleware.</param>
    /// <returns>An instance of <see cref="ApiLoggingOptionsBuilder"/> with the excluded paths configured.</returns>
    /// <remarks>
    /// <para>
    /// This method allows you to specify specific paths to be excluded from logging.
    /// </para>
    /// <para>
    /// You can configure the first part of the path (e.g., "/api") to exclude all sub-paths under that path.
    /// </para>
    /// </remarks>
    public ApiLoggingOptionsBuilder WithExcludePaths(params string[] excludedPaths)
    {
        var paths = _options.ExcludedPaths?.ToList() ?? [];
        paths.AddRange(excludedPaths);
        _options.ExcludedPaths = paths.ToArray();
        return this;
    }

    /// <summary>
    /// Configures the logging options to exclude the specified files from being logged.
    /// </summary>
    /// <param name="excludedFiles">An array of file names to exclude from logging. These files will not be logged by the API logging middleware.</param>
    /// <returns>An instance of <see cref="ApiLoggingOptionsBuilder"/> with the excluded files configured.</returns>
    /// <remarks>
    /// <para>
    /// This method allows you to specify specific file names to be excluded from logging.
    /// </para>
    /// <para>Supports wildcards ('*') or regular expressions. (Wildcards will be handled via regular expressions, too)</para>
    /// </remarks>
    public ApiLoggingOptionsBuilder WithExcludedFiles(params string[] excludedFiles)
    {
        var files = _options.ExcludedFiles?.ToList() ?? [];
        files.AddRange(excludedFiles);
        _options.ExcludedFiles = files.ToArray();
        return this;
    }

    /// <summary>
    /// Builds and returns an instance of <see cref="ApiLoggingOptions"/> with the current configuration settings.
    /// </summary>
    /// <returns>A configured instance of <see cref="ApiLoggingOptions"/> representing the specified logging options.</returns>
    public ApiLoggingOptions Build()
    {
        return _options;
    }
}

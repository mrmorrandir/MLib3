using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace MLib3.AspNetCore;

/// <summary>
/// Middleware for logging HTTP requests and responses within the application.
/// </summary>
/// <remarks>
/// This middleware captures detailed information about incoming HTTP requests
/// and outgoing responses for debugging and auditing purposes. Specific paths
/// can be excluded from logging based on the configuration.
/// </remarks>
public class ApiLoggingMiddleware : IMiddleware
{
    private readonly string[] _excludedPaths;
    private readonly Regex[] _excludedFileRegexes;
    
    private readonly ILogger<ApiLoggingMiddleware> _logger;
    private readonly IApiLoggingService _apiLoggingService;

    /// <summary>
    /// Middleware for logging HTTP requests and responses within the application.
    /// </summary>
    /// <remarks>
    /// This middleware captures detailed information about incoming HTTP requests
    /// and outgoing responses for debugging and auditing purposes. It supports
    /// configuration to exclude specific paths from logging via the provided options.
    /// </remarks>
    public ApiLoggingMiddleware(
        ILogger<ApiLoggingMiddleware> logger,
        IApiLoggingService apiLoggingService,
        IOptions<ApiLoggingOptions> options)
    {
        _logger = logger;
        _apiLoggingService = apiLoggingService;
        _excludedPaths = options.Value.ExcludedPaths ?? [];
        // Convert wildcard patterns to Regex
        // "hallo*.js" -> "^hallo.*\.js$"
        _excludedFileRegexes = (options.Value.ExcludedFiles ?? [])
            .Select(pattern => new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$", RegexOptions.IgnoreCase | RegexOptions.Compiled))
            .ToArray();
    }

    /// <summary>
    /// Handles an HTTP request by logging request and response details, skipping excluded paths,
    /// and measuring execution time. Stores the log data in the database and provides diagnostics.
    /// </summary>
    /// <param name="httpContext">The context object for the HTTP request.</param>
    /// <param name="next">The next delegate in the middleware pipeline.</param>
    /// <returns>A task representing the completion of the middleware execution.</returns>
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            await next(httpContext);
            return;
        }

        // Skip logging for excluded paths
        if (_excludedPaths.Any(p => httpContext.Request.Path.Value?.StartsWith(p, StringComparison.OrdinalIgnoreCase) == true))
        {
            await next(httpContext);
            return;
        }
        
        // Skip logging for excluded files (with wildcard support - e.g. "hallo*.js" should find "hallowelt.js" and "hallo-welt.js")
        if (_excludedFileRegexes.Any(r => r.IsMatch(httpContext.Request.Path.Value ?? "")))
        {
            await next(httpContext);
            return;
        }
        
        // Read request body
        httpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        httpContext.Request.Body.Position = 0;

        var originalBodyStream = httpContext.Response.Body;
        using var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;
        
        // Measure execution time
        var stopwatch = Stopwatch.StartNew();
        // Execute the next middleware in the pipeline
        await next(httpContext);
        stopwatch.Stop();

        // Read response body
        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        
        // Log to database
        var log = new ApiLog
        {
            Timestamp = DateTime.UtcNow,
            Method = httpContext.Request.Method,
            Path = httpContext.Request.Path,
            QueryString = httpContext.Request.QueryString.Value ?? "",
            RequestJson = requestBody,
            ResponseJson = responseText,
            StatusCode = httpContext.Response.StatusCode,
            DurationMilliseconds = stopwatch.ElapsedMilliseconds
        };
        
        _logger.LogInformation("API {Method} {Path} responded {StatusCode} in {Duration}ms - Request: {RequestJson} Response: {Response}", log.Method, log.Path, log.StatusCode, log.DurationMilliseconds, log.RequestJson, log.ResponseJson);
        await _apiLoggingService.LogAsync(log, httpContext.RequestAborted);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}

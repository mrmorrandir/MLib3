using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using MLib3.Logging;

namespace MLib3.AspNetCore;

/// <summary>
/// Middleware for logging sanitized HTTP requests and responses within the application.
/// </summary>
public class ApiLoggingMiddleware : IMiddleware
{
    private readonly string[] _excludedPaths;
    private readonly Regex[] _excludedFileRegexes;
    private readonly ILogger<ApiLoggingMiddleware> _logger;
    private readonly IApiLoggingService _apiLoggingService;
    private readonly ILogPayloadSanitizer _payloadSanitizer;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiLoggingMiddleware(
        ILogger<ApiLoggingMiddleware> logger,
        IApiLoggingService apiLoggingService,
        IOptions<ApiLoggingOptions> options,
        ILogPayloadSanitizer payloadSanitizer,
        IOptions<JsonOptions> jsonOptions)
    {
        _logger = logger;
        _apiLoggingService = apiLoggingService;
        _payloadSanitizer = payloadSanitizer;
        _serializerOptions = jsonOptions.Value.SerializerOptions;
        _excludedPaths = options.Value.ExcludedPaths ?? [];
        _excludedFileRegexes = (options.Value.ExcludedFiles ?? [])
            .Select(pattern => new Regex(
                "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled))
            .ToArray();
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            await next(httpContext);
            return;
        }

        if (_excludedPaths.Any(path =>
                httpContext.Request.Path.Value?.StartsWith(path, StringComparison.OrdinalIgnoreCase) == true))
        {
            await next(httpContext);
            return;
        }

        if (_excludedFileRegexes.Any(regex => regex.IsMatch(httpContext.Request.Path.Value ?? "")))
        {
            await next(httpContext);
            return;
        }

        httpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        httpContext.Request.Body.Position = 0;

        var originalBodyStream = httpContext.Response.Body;
        using var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;

        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(httpContext);
            stopwatch.Stop();

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            var payloadTypes = ApiLogPayloadTypeResolver.Resolve(
                httpContext.GetEndpoint(),
                httpContext.Response.StatusCode);
            var sanitizedRequest = SanitizeBody(
                requestBody,
                httpContext.Request.ContentType,
                payloadTypes.RequestType,
                "request");
            var sanitizedResponse = SanitizeBody(
                responseText,
                httpContext.Response.ContentType,
                payloadTypes.ResponseType,
                "response");

            var log = new ApiLog
            {
                Timestamp = DateTime.UtcNow,
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path,
                QueryString = httpContext.Request.QueryString.Value ?? "",
                RequestJson = sanitizedRequest,
                ResponseJson = sanitizedResponse,
                StatusCode = httpContext.Response.StatusCode,
                DurationMilliseconds = stopwatch.ElapsedMilliseconds
            };

            _logger.LogInformation(
                "API {Method} {Path} responded {StatusCode} in {Duration}ms - Request: {RequestJson} Response: {Response}",
                log.Method,
                log.Path,
                log.StatusCode,
                log.DurationMilliseconds,
                log.RequestJson,
                log.ResponseJson);
            await _apiLoggingService.LogAsync(log, httpContext.RequestAborted);

            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            httpContext.Response.Body = originalBodyStream;
        }
    }

    private string SanitizeBody(string body, string? contentType, Type? payloadType, string payloadKind)
    {
        if (string.IsNullOrWhiteSpace(body))
            return body;

        if (!IsJsonContentType(contentType))
            return "";

        var result = _payloadSanitizer.SanitizeJson(body, payloadType, _serializerOptions);
        if (result.IsSuccess)
            return result.IsExcluded ? "" : result.Payload ?? "";

        _logger.LogWarning(
            "API {PayloadKind} payload sanitization failed; payload was omitted",
            payloadKind);
        return "";
    }

    private static bool IsJsonContentType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        return contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)
               || contentType.Contains("+json", StringComparison.OrdinalIgnoreCase);
    }
}

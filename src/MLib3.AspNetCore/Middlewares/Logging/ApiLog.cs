namespace MLib3.AspNetCore;

/// <summary>
/// Represents a log entry for an API request and response.
/// </summary>
/// <remarks>
/// This record encapsulates detailed information about an API interaction,
/// including the HTTP method, request path, query string, request and response payloads,
/// status code, and the execution time of the request. It is used for tracking
/// and auditing API behavior within the application.
/// </remarks>
public record ApiLog
{
    /// <summary>
    /// Gets the timestamp representing the UTC date and time at which the API request and response were logged.
    /// </summary>
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets the HTTP method (e.g., GET, POST, PUT, DELETE) of the API request.
    /// </summary>
    public required string Method { get; init; }

    /// <summary>
    /// Gets the relative URL path of the API request that was logged.
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    /// Gets the query string of the HTTP request, including any parameters and their values,
    /// as represented in the original API call.
    /// </summary>
    public required string QueryString { get; init; }

    /// <summary>
    /// Gets the raw JSON content of the API request body.
    /// </summary>
    public required string RequestJson { get; init; }

    /// <summary>
    /// Gets the JSON representation of the HTTP response body captured during the API request processing.
    /// </summary>
    public required string ResponseJson { get; init; }

    /// <summary>
    /// Gets the HTTP status code returned by the API in the response to the logged request.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Gets the duration of the API request and response lifecycle in milliseconds.
    /// </summary>
    public long DurationMilliseconds { get; init; }
}
namespace MLib3.AspNetCore.Clients;

/// <summary>
/// Base exception for API clients, providing HTTP status code and response content.
/// </summary>
/// <remarks>
/// This class should be configured in NSwagStudio as the base exception.
/// </remarks>
public class ApiClientException : Exception
{
    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    public int StatusCode { get; private set; }

    /// <summary>
    /// The content of the HTTP response as a string.
    /// </summary>
    public string? Response { get; private set; }

    /// <summary>
    /// The HTTP headers of the response.
    /// </summary>
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClientException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="response">The response content.</param>
    /// <param name="headers">The HTTP headers.</param>
    /// <param name="innerException">The underlying exception.</param>
    public ApiClientException(
        string message,
        int statusCode,
        string? response,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        Exception innerException)
        : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response[..(response.Length >= 512 ? 512 : response.Length)]), innerException)
    {
        StatusCode = statusCode;
        Response = response ;
        Headers = headers;
    }

    /// <summary>
    /// Returns a string representation of the exception, including the response content.
    /// </summary>
    /// <returns>A string describing the exception.</returns>
    public override string ToString()
    {
        return $"HTTP Response: \n\n{Response ?? "(null)"}\n\n{base.ToString()}";
    }
}
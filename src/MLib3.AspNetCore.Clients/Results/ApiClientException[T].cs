namespace MLib3.AspNetCore.Clients;

/// <summary>
/// Generic API exception containing a deserialized result object.
/// </summary>
/// <typeparam name="TResult">The type of the deserialized object (e.g., <see cref="ErrorResponse"/>).</typeparam>
public partial class ApiClientException<TResult> : ApiClientException
{
    /// <summary>
    /// The deserialized result object of the API response.
    /// </summary>
    public TResult Result { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClientException{TResult}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="response">The response content as a string.</param>
    /// <param name="headers">The HTTP headers.</param>
    /// <param name="result">The deserialized result.</param>
    /// <param name="innerException">The underlying exception.</param>
    public ApiClientException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }
}
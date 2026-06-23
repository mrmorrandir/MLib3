namespace MLib3.AspNetCore;

/// <summary>
/// Represents an error response containing a collection of error details.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the collection of error details.
    /// </summary>
    /// <remarks>
    /// This property contains a list of errors, where each error is represented by
    /// an <see cref="ErrorDetail"/> instance. It is used to encapsulate multiple
    /// related error messages or causes within the error response.
    /// </remarks>
    public List<ErrorDetail> Errors { get; set; } = new();
}
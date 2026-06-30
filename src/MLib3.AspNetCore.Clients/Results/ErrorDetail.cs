namespace MLib3.AspNetCore.Clients;

/// <summary>
/// Represents a detailed error object containing an error message and potential causes.
/// </summary>
/// <remarks>
/// This class is used to encapsulate information about an error, including its message
/// and any underlying errors that caused it. The <see cref="CausedBy"/> property contains
/// a collection of nested errors, allowing for a detailed representation of error hierarchies.
/// </remarks>
public class ErrorDetail
{
    /// <summary>
    /// The error message associated with the error detail.
    /// </summary>
    public string Message { get; set; } = "Error";

    /// <summary>
    /// The collection of detailed errors that caused the current error.
    /// </summary>
    /// <remarks>
    /// Each element in this collection is itself an instance of <see cref="ErrorDetail"/>.
    /// </remarks>
    public List<ErrorDetail> CausedBy { get; set; } = new();

    /// <summary>
    /// Additional metadata associated with the error.
    /// </summary>
    public List<ErrorMeta> Meta { get; set; } = new();
}
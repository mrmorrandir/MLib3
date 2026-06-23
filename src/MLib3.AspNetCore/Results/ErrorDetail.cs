namespace MLib3.AspNetCore;

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
    /// <remarks>
    /// This property holds a human-readable message describing the error that occurred.
    /// It is commonly used to provide context or information to the user or developer
    /// about the nature of the error.
    /// </remarks>
    public string Message { get; set; } = "Error";

    /// <summary>
    /// The collection of detailed errors that caused the current error.
    /// </summary>
    /// <remarks>
    /// This property represents a list of related errors that contribute to or explain
    /// the occurrence of the current error. It is used to establish a hierarchy of errors,
    /// enabling a comprehensive understanding of the root causes and their context.
    /// Each error detail in this collection is itself an instance of <see cref="ErrorDetail"/>.
    /// </remarks>
    public List<ErrorDetail> CausedBy { get; set; } = new();

    /// <summary>
    /// Additional metadata associated with the error.
    /// </summary>
    /// <remarks>
    /// This property allows for the inclusion of extra information about the error in the form of key-value pairs.
    /// It can be used to provide additional context, such as error codes, timestamps, or any other relevant data that may assist in diagnosing or understanding the error.
    /// </remarks>
    public List<ErrorMeta> Meta { get; set; } = new();
}
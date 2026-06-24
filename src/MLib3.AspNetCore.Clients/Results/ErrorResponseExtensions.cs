namespace MLib3.AspNetCore.Clients;

/// <summary>
/// Provides extension methods for converting <see cref="ErrorResponse"/> to <see cref="Result"/>.
/// </summary>
public static class ErrorResponseExtensions
{
    /// <summary>
    /// Converts an <see cref="ErrorResponse"/> to a FluentResults <see cref="Result"/>.
    /// </summary>
    /// <param name="errorResponse">The error response to convert.</param>
    /// <returns>A <see cref="Result"/> containing the mapped errors.</returns>
    public static Result ToFluentResult(this ErrorResponse errorResponse)
    {
        var result = new Result();
        foreach (var errorDetail in errorResponse.Errors)
            result.WithError(errorDetail.GetError());
        if (errorResponse.Errors.Count == 0)
            result.WithError(new Error("Unknown error"));
        return result;
    }

    /// <summary>
    /// Maps an <see cref="ErrorDetail"/> to a FluentResults <see cref="Error"/>.
    /// </summary>
    /// <param name="errorDetail">The error detail to map.</param>
    /// <returns>A <see cref="Error"/> object with metadata and reasons.</returns>
    private static Error GetError(this ErrorDetail errorDetail)
    {
        var error = new Error(errorDetail.Message);
        foreach(var meta in errorDetail.Meta)
            error.WithMetadata(meta.Key, meta.Value);
        foreach(var detail in errorDetail.CausedBy)
            error.CausedBy(detail.GetError());
        return error;
    }
}
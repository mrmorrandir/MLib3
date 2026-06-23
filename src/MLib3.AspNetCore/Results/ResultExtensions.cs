
namespace MLib3.AspNetCore;

/// <summary>
///     Provides extension methods for converting FluentResults to web-compatible results and error responses.
/// </summary>
/// <remarks>
///     This works nicely together with the <c>Mediator.SourceGenerator</c> when the commands and queries return FluentResults.
/// </remarks>
public static class ResultExtensions
{
    /// <summary>
    ///     Converts a Task containing a FluentResult with value to a web-compatible IResult.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="resultTask">The task containing the FluentResult to convert.</param>
    /// <returns>
    ///     Returns Ok(value) if the operation was successful, or BadRequest with error details if it failed.
    /// </returns>
    public static async Task<IResult> ToWebResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.GetResponse());
    }

    /// <summary>
    /// Converts a FluentResult with a value to a web-compatible IResult.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The FluentResult to convert.</param>
    /// <returns>
    /// Returns Ok() if the operation was successful, or BadRequest with error details if it failed.
    /// </returns>
    public static IResult ToWebResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.GetResponse());
    }

    /// <summary>
    ///     Converts a Task containing a FluentResult to a web-compatible IResult.
    /// </summary>
    /// <param name="resultTask">The task containing the FluentResult to convert.</param>
    /// <returns>
    ///     Returns Ok if the operation was successful, or BadRequest with error details if it failed.
    /// </returns>
    public static async Task<IResult> ToWebResult(this Task<Result> resultTask)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(result.GetResponse());
    }

    /// <summary>
    /// Converts a FluentResult with a value to a web-compatible IResult.
    /// </summary>
    /// <param name="result">The FluentResult to convert.</param>
    /// <returns>
    /// Returns Ok() if the operation was successful, or BadRequest with error details if it failed.
    /// </returns>
    public static IResult ToWebResult(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(result.GetResponse());
    }

    /// <summary>
    ///     Converts a ValueTask containing a FluentResult with value to a web-compatible IResult.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="resultValueTask">The ValueTask containing the FluentResult to convert.</param>
    /// <returns>
    ///     Returns Ok(value) if the operation was successful, or BadRequest with error details if it failed.
    /// </returns>
    public static async ValueTask<IResult> ToWebResult<T>(this ValueTask<Result<T>> resultValueTask)
    {
        var result = await resultValueTask;
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.GetResponse());
    }

    /// <summary>
    ///     Converts a ValueTask containing a FluentResult to a web-compatible IResult.
    /// </summary>
    /// <param name="resultValueTask">The ValueTask containing the FluentResult to convert.</param>
    /// <returns>
    ///     Returns Ok if the operation was successful, or BadRequest with error details if it failed.
    /// </returns>
    public static async ValueTask<IResult> ToWebResult(this ValueTask<Result> resultValueTask)
    {
        var result = await resultValueTask;
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(result.GetResponse());
    }

    /// <summary>
    ///     Converts a FluentResults ResultBase to an ErrorResponse object.
    /// </summary>
    /// <param name="resultBase">The ResultBase to convert.</param>
    /// <returns>
    ///     An ErrorResponse containing a list of error details extracted from the ResultBase.
    /// </returns>
    public static ErrorResponse GetResponse(this ResultBase resultBase)
    {
        return new ErrorResponse
        {
            Errors = resultBase.Errors.Select(e => e.GetErrorDetail()).ToList()
        };
    }

    /// <summary>
    ///     Converts a FluentResults IError to an ErrorDetail object.
    /// </summary>
    /// <param name="error">The IError to convert.</param>
    /// <returns>
    ///     An ErrorDetail containing the error message and any nested errors (reasons).
    /// </returns>
    public static ErrorDetail GetErrorDetail(this IError error)
    {
        return new ErrorDetail
        {
            Message = error.Message,
            CausedBy = error.Reasons.Select(r => r.GetErrorDetail()).ToList(),
            Meta = error.Metadata.Select(kv => new ErrorMeta { Key = kv.Key, Value = kv.Value?.ToString() ?? "null" }).ToList()
        };
    }
}
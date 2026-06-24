namespace MLib3.AspNetCore.Clients;

/// <summary>
/// Provides extension methods for <see cref="Task"/> to simplify result handling.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Awaits the task and converts it into a <see cref="Result{T}"/>. 
    /// Handles <see cref="ApiClientException{ErrorResponse}"/> by converting it to fluent errors.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="task">The task to be converted into a result.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the value of the completed task,
    /// or an error result if an exception occurs.
    /// </returns>
    public static async Task<Result<T>> ToResult<T>(this Task<T> task)
    {
        try
        {
            return await task;
        }
        catch (ApiClientException<ErrorResponse> apiException)
        {
            var errorResponse = apiException.Result;
            return errorResponse!.ToFluentResult();
        }
        catch (ApiClientException apiException)
        {
            return new ExceptionalError(apiException);
        }
        catch (Exception ex)
        {
            return new ExceptionalError(ex);
        }
    }
    
    /// <summary>
    /// Awaits the task and converts it into a <see cref="Result"/>.
    /// Handles <see cref="ApiClientException{ErrorResponse}"/> by converting it to fluent errors.
    /// </summary>
    /// <param name="task">The task to be converted into a result.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating success or containing errors if an exception occurs.
    /// </returns>
    public static async Task<Result> ToResult(this Task task)
    {
        try
        {
            await task;
            return Result.Ok();
        }
        catch (ApiClientException<ErrorResponse> apiException)
        {
            var errorResponse = apiException.Result;
            return errorResponse!.ToFluentResult();
        }
        catch (ApiClientException apiException)
        {
            return new ExceptionalError(apiException);
        }
        catch (Exception ex)
        {
            return new ExceptionalError(ex);
        }
    }
}
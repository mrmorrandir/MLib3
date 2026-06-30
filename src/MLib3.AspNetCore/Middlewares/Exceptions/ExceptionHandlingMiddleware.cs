namespace MLib3.AspNetCore;

/// <summary>
/// Middleware for handling exceptions in the HTTP request pipeline.
/// </summary>
/// <remarks>
/// This middleware catches unhandled exceptions that occur during request processing,
/// logs the exception details, and returns a standardized error response to the client.
///
/// This is the last line of defense for unhandled exceptions in the application.
/// It ensures that the application does not crash and provides meaningful feedback to the client.
/// </remarks>
public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    /// Middleware for handling unhandled exceptions in the HTTP request pipeline.
    /// </summary>
    /// <remarks>
    /// This middleware intercepts unhandled exceptions that occur during request processing,
    /// logs detailed information about the exception, and sends a structured error response
    /// to the client. It ensures a consistent and meaningful error-handling experience while
    /// preventing the application from crashing on runtime exceptions.
    /// </remarks>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    /// <summary>
    /// Handles HTTP requests and catches unhandled exceptions that occur during the request pipeline processing.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing. If an unhandled exception occurs, it logs the exception and constructs a standardized error response.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing the request");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var response = Result.Fail(new ExceptionalError(ex)).GetResponse();
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
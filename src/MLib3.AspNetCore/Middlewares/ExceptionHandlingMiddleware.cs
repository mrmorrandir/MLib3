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

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    
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
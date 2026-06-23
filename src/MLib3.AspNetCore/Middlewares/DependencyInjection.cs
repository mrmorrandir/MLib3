using MLib3.AspNetCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static partial class DependencyInjection
{
    /// <summary>
    /// Adds the ExceptionHandlingMiddleware to the application's request processing pipeline.
    /// This middleware captures unhandled exceptions, logs the error details,
    /// and returns a standardized error response to the client.
    /// </summary>
    /// <param name="app">The instance of <see cref="IApplicationBuilder"/> used to configure the application's request pipeline.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance with the middleware added to the pipeline.</returns>
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }   
}
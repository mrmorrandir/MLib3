using MLib3.AspNetCore;
using MLib3.AspNetCore.Exceptions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding custom middleware and services to the ASP.NET Core Dependency Injection container.
/// </summary>
/// <remarks>
/// This static class contains methods to extend the functionality of the ASP.NET Core application by
/// registering middleware and other dependencies. It simplifies the process of incorporating custom
/// components into the application's request processing pipeline or service collection.
/// </remarks>
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
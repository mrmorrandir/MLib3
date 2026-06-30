using MLib3.AspNetCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static partial class DependencyInjection
{
    /// <summary>
    /// Registers the API logging middleware with the application's dependency injection container.
    /// </summary>
    /// <param name="services">The collection of service descriptors to which the middleware will be added.</param>
    /// <returns>The modified service collection with the API logging middleware registered.</returns>
    /// <remarks>
    /// The API logging middleware is configured using options bound from the "ApiLogging" configuration section.
    /// </remarks>
    /// <seealso cref="ApiLoggingOptions"/>
    /// <seealso cref="ApiLoggingOptionsBuilder"/>
    /// <seealso cref="AddApiLoggingMiddleware(IServiceCollection, Action{ApiLoggingOptionsBuilder})"/>
    /// <seealso cref="UseApiLoggingMiddleware(IApplicationBuilder)"/>
    public static IServiceCollection AddApiLoggingMiddleware(this IServiceCollection services)
    {
        services.AddOptions<ApiLoggingOptions>()
            .BindConfiguration("ApiLogging")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddScoped<IApiLoggingService, ApiLoggingService>();
        services.AddScoped<ApiLoggingMiddleware>();
        return services;
    }

    /// <summary>
    /// Adds the API logging middleware to the service collection and configures its options.
    /// </summary>
    /// <param name="services">The collection of service descriptors to which the middleware will be added.</param>
    /// <param name="configureOptions">An action to configure the options for the API logging middleware.</param>
    /// <returns>The modified service collection with the API logging middleware registered and configured.</returns>
    public static IServiceCollection AddApiLoggingMiddleware(this IServiceCollection services, Action<ApiLoggingOptionsBuilder> configureOptions)
    {
        var builder = new ApiLoggingOptionsBuilder();
        configureOptions(builder);
        var options = builder.Build();

        services.AddOptions<ApiLoggingOptions>()
            .Configure(opts =>
            {
                opts.ExcludedPaths = options.ExcludedPaths;
                opts.ExcludedFiles = options.ExcludedFiles;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IApiLoggingService, ApiLoggingService>();
        services.AddScoped<ApiLoggingMiddleware>();
        return services;
    }

    /// <summary>
    /// Registers an API log handler that receives captured <see cref="ApiLog"/> entries.
    /// </summary>
    /// <typeparam name="THandler">The handler implementation type.</typeparam>
    /// <param name="services">The collection of service descriptors to which the handler will be added.</param>
    /// <returns>The modified service collection with the handler registered.</returns>
    public static IServiceCollection AddApiLogHandler<THandler>(this IServiceCollection services)
        where THandler : class, IApiLogHandler
    {
        services.AddScoped<IApiLogHandler, THandler>();
        return services;
    }

    /// <summary>
    /// Adds the API logging middleware to the application's request processing pipeline.
    /// </summary>
    /// <param name="builder">The application builder that configures the middleware pipeline.</param>
    /// <returns>The application builder with the API logging middleware added.</returns>
    /// <remarks>
    /// Be aware of the order in which middleware is added to the pipeline.
    /// </remarks>
    public static IApplicationBuilder UseApiLoggingMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ApiLoggingMiddleware>();
        return builder;
    }
}

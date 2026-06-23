using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

namespace MLib3.AspNetCore;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all implementations of the <see cref="IEndpoint"/> interface from the provided assemblies
    /// into the dependency injection container as singleton services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the endpoints will be registered.</param>
    /// <param name="assemblies">An array of assemblies to search for <see cref="IEndpoint"/> implementations.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    /// <remarks>
    /// <para>
    /// The endpoints are registered as singleton services, as they are typically used only once 
    /// during application startup to register routes.
    /// </para>
    /// <para>
    /// If no <paramref name="assemblies"/> are provided, the current entry assembly will be used.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = [Assembly.GetEntryAssembly()!];
        
        var endpoints = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false})
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type))
            .ToList();
        
        foreach (var endpoint in endpoints)
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IEndpoint), endpoint));

        return services;
    }

    /// <summary>
    /// Configures all registered <see cref="IEndpoint"/> implementations by mapping their routes
    /// to the specified <see cref="WebApplication"/> instance.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoints will be mapped.</param>
    /// <returns>The configured <see cref="WebApplication"/> instance.</returns>
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IEndpoint>();
        foreach (var endpoint in endpoints)
            endpoint.Register(app);

        return app;
    }
}
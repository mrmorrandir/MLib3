using Microsoft.Extensions.DependencyInjection;

namespace MLib3.System;

/// <summary>
/// Provides extension methods to register dependencies in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers a Base32 converter - <see cref="IBase32Converter"/> - implementation as a singleton service in the dependency injection container.
    /// </summary>
    /// <param name="services">The dependency injection service collection to add the Base32 converter to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddBase32Converter(this IServiceCollection services)
    {
        services.AddSingleton<IBase32Converter, Rfc4648Base32Converter>();
        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace MLib3.AspDotNet.ApiKeys;

public static class DependencyInjection
{
    public static IServiceCollection AddApiKeys(this IServiceCollection services, Action<ApiKeysConfigurationBuilder> options)
    {
        var optionsBuilder = new ApiKeysConfigurationBuilder(services);
        options.Invoke(optionsBuilder);
        optionsBuilder.Build();
        return services;
    }
}
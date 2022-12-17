using Microsoft.Extensions.DependencyInjection;

namespace MLib3.AspDotNet.ApiKeys;

public static class DependencyInjection
{
    public static IServiceCollection AddApiKeys(this IServiceCollection services, Action<ApiKeysConfigurationBuilder> config)
    {
        var builder = new ApiKeysConfigurationBuilder(services);
        config(builder);
        return builder.Build();
    }
}
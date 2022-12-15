using Microsoft.Extensions.DependencyInjection;
using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys;

public static class ApiKeysConfigurationBuilderExtensions
{
    public static void UseApiKey(this ApiKeysConfigurationBuilder builder, string apiKey)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient<IApiKeyValidator>(sp => new StringApiKeyValidator(apiKey));
        });
    }
    
    public static void UseEnvironmentApiKey(this ApiKeysConfigurationBuilder builder, string environmentVariableName = "HOST_API_KEY")
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient<IApiKeyValidator>(sp => new EnvironmentApiKeyValidator(environmentVariableName));
        });
    }
}
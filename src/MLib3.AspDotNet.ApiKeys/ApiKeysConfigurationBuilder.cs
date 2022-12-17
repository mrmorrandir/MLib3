using Microsoft.Extensions.DependencyInjection;

namespace MLib3.AspDotNet.ApiKeys;

public class ApiKeysConfigurationBuilder : ConfigurationBuilder<ApiKeysConfigurationBuilder>
{
    public ApiKeysConfigurationBuilder(IServiceCollection services) : base(services)
    {

    }
}
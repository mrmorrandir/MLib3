using Microsoft.Extensions.DependencyInjection;

namespace MLib3.AspDotNet.ApiKeys;

public class ApiKeysConfigurationBuilder
{
    private readonly IServiceCollection _services;
    private readonly List<Action<IServiceCollection>> _configureServices = new();

    public ApiKeysConfigurationBuilder(IServiceCollection services) 
    {
        _services = services;
    }

    public ApiKeysConfigurationBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        _configureServices.Add(configureServices);
        return this;
    }

    public IServiceCollection Build()
    {
        foreach (var configureService in _configureServices)
            configureService(_services);
        return _services;
    }
}
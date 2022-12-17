using Microsoft.Extensions.DependencyInjection;

namespace MLib3.AspDotNet.ApiKeys;

public class ConfigurationBuilder<TConfigurationBuilder> where TConfigurationBuilder: class
{
    private readonly IServiceCollection _services;
    private readonly List<Action<IServiceCollection>> _configureServices = new();

    public ConfigurationBuilder(IServiceCollection services) 
    {
        _services = services;
    }

    public TConfigurationBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        _configureServices.Add(configureServices);
        return (this as TConfigurationBuilder)!;
    }
    
    public IServiceCollection Build()
    {
        foreach (var configureService in _configureServices)
            configureService(_services);
        return _services;
    }
}
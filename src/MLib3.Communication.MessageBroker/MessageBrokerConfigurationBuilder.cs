using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Communication.MessageBroker;

public class MessageBrokerConfigurationBuilder
{
    private readonly IServiceCollection _services;
    private readonly List<Action<IServiceCollection>> _configureServices = new();

    public MessageBrokerConfigurationBuilder(IServiceCollection services) 
    {
        _services = services;
    }

    public MessageBrokerConfigurationBuilder ConfigureServices(Action<IServiceCollection> configureServices)
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
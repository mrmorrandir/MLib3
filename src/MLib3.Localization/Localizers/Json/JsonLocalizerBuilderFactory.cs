using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Localization.Localizers.Json;

public class JsonLocalizerBuilderFactory : IJsonLocalizerBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JsonLocalizerBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IJsonLocalizerBuilder Create()
    {
        return ActivatorUtilities.CreateInstance<JsonLocalizerBuilder>(_serviceProvider);
    }
}
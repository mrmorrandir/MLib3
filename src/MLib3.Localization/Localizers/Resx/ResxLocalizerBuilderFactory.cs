using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Localization.Localizers.Resx;

public class ResxLocalizerBuilderFactory : IResxLocalizerBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResxLocalizerBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IResxLocalizerBuilder Create()
    {
        return ActivatorUtilities.CreateInstance<ResxLocalizerBuilder>(_serviceProvider);
    }
}
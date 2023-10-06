using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Resx;

public class ResxLocalizerFactory : IResxLocalizerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResxLocalizerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public ILocalizer Create(string resource, CultureInfo cultureInfo, Assembly? assembly = null)
    {
        return assembly is null
            ? ActivatorUtilities.CreateInstance<ResxLocalizer>(_serviceProvider,
                resource,
                cultureInfo)
            : ActivatorUtilities.CreateInstance<ResxLocalizer>(_serviceProvider,
                resource,
                cultureInfo,
                assembly);
    }
}
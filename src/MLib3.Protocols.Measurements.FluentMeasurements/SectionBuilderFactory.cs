using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class SectionBuilderFactory : ISectionBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SectionBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ISectionBuilder Create()
    {
        return ActivatorUtilities.CreateInstance<SectionBuilder>(_serviceProvider);
    }

    public ISectionBuilder Create(ISectionSetting setting)
    {
        return ActivatorUtilities.CreateInstance<SectionBuilder>(_serviceProvider, setting);
    }
}
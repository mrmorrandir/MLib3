using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ResultsBuilderFactory : IResultsBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResultsBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IResultsBuilder Create()
    {
        return ActivatorUtilities.CreateInstance<ResultsBuilder>(_serviceProvider);
    }
}
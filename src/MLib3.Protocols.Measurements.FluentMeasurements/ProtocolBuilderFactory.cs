namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProtocolBuilderFactory : IProtocolBuilderFactory
{
    private readonly IResultsBuilderFactory _resultsBuilderFactory;
    private readonly IMetaBuilderFactory _metaBuilderFactory;
    private readonly IProductBuilderFactory _productBuilderFactory;

    public ProtocolBuilderFactory(IResultsBuilderFactory resultsBuilderFactory, IMetaBuilderFactory metaBuilderFactory, IProductBuilderFactory productBuilderFactory)
    {
        _resultsBuilderFactory = resultsBuilderFactory;
        _metaBuilderFactory = metaBuilderFactory;
        _productBuilderFactory = productBuilderFactory;
    }
    
    public IProtocolBuilder Create()
    {
        return new ProtocolBuilder(_metaBuilderFactory, _productBuilderFactory, _resultsBuilderFactory);
    }
}
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProtocolBuilder : IProtocolBuilder
{
    private readonly IMetaBuilderFactory _metaBuilderFactory;
    private readonly IProductBuilderFactory _productBuilderFactory;
    private readonly IResultsBuilderFactory _resultsBuilderFactory;
    private readonly Protocol _protocol;
    private readonly List<Action> _builders = new();
    private bool _isMetaSet;
    private bool _isProductSet;
    private bool _isResultsSet;

    public ProtocolBuilder(IMetaBuilderFactory metaBuilderFactory, IProductBuilderFactory productBuilderFactory, IResultsBuilderFactory resultsBuilderFactory)
    {
        _metaBuilderFactory = metaBuilderFactory;
        _productBuilderFactory = productBuilderFactory;
        _resultsBuilderFactory = resultsBuilderFactory;
        _protocol = new Protocol();
    }
    
    public IProtocolBuilder Product(Action<IProductBuilder> productBuilder)
    {
        if (_isProductSet)
            throw new InvalidOperationException($"{nameof(_protocol.Product)} is already set");
        var builder = _productBuilderFactory.Create();
        productBuilder(builder);
        _builders.Add(() =>
        {
            _protocol.Product = builder.Build();
        });
        _isProductSet = true;
        return this;
    }
    
    public IProtocolBuilder Meta(Action<IMetaBuilder> metaBuilder)
    {
        if (_isMetaSet)
            throw new InvalidOperationException($"{nameof(_protocol.Meta)} is already set");
        var builder = _metaBuilderFactory.Create();
        metaBuilder(builder);
        _builders.Add(() =>
        {
            _protocol.Meta = builder.Build();
        });
        _isMetaSet = true;
        return this;
    }
    
    public IProtocolBuilder Results(Action<IResultsBuilder> resultsBuilder)
    {
        if (_isResultsSet)
            throw new InvalidOperationException($"{nameof(_protocol.Results)} is already set");
        var builder = _resultsBuilderFactory.Create();
        resultsBuilder(builder);
        _builders.Add(() =>
        {
            _protocol.Results = builder.Build();
        });
        _isResultsSet = true;
        return this;
    }
    
    public IProtocol Build()
    {
        foreach (var builder in _builders)
            builder();
        if (_protocol.Product is null || !_isProductSet)
            throw new InvalidOperationException($"{nameof(_protocol.Product)} is not set");
        if (_protocol.Meta is null || !_isMetaSet)
            throw new InvalidOperationException($"{nameof(_protocol.Meta)} is not set");
        if (_protocol.Results is null || !_isResultsSet)
            throw new InvalidOperationException($"{nameof(_protocol.Results)} is not set");
        
        return _protocol;
    }
}
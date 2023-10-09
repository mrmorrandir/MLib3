// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProtocolBuilder : IProtocolBuilder
{
    private readonly Protocol _protocol;
    private readonly List<Action> _builders = new List<Action>();
    private bool _isMetaSet;
    private bool _isProductSet;
    private bool _isResultsSet;

    private ProtocolBuilder()
    {
        _protocol = new Protocol();
    }
    
    public static IProtocolBuilder Create() => new ProtocolBuilder();
    
    public IProtocolBuilder Product(Action<IProductBuilder> productBuilder)
    {
        var builder = ProductBuilder.Create();
        productBuilder(builder);
        _builders.Add(() =>
        {
            _protocol.Product = builder.Build();
        });
        _isProductSet = true;
        return this;
    }
    
    public IProtocolBuilder Product(IProduct product)
    {
        _builders.Add(() =>
        {
            _protocol.Product = product;
        });
        _isProductSet = true;
        return this;
    }
    
    public IProtocolBuilder Meta(Action<IMetaBuilder> metaBuilder)
    {
        var builder = MetaBuilder.Create();
        metaBuilder(builder);
        _builders.Add(() =>
        {
            _protocol.Meta = builder.Build();
        });
        _isMetaSet = true;
        return this;
    }
    
    public IProtocolBuilder Meta(IMeta meta)
    {
        _builders.Add(() =>
        {
            _protocol.Meta = meta;
        });
        _isMetaSet = true;
        return this;
    }
    
    public IProtocolBuilder Results(Action<IResultsBuilder> resultsBuilder)
    {
        var builder = ResultsBuilder.Create();
        resultsBuilder(builder);
        _builders.Add(() =>
        {
            _protocol.Results = builder.Build();
        });
        _isResultsSet = true;
        return this;
    }
    
    public IProtocolBuilder Results(IResults results)
    {
        _builders.Add(() =>
        {
            _protocol.Results = results;
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
namespace MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

public interface IProtocolBuilder
{
    IProtocolBuilder Product(Action<IProductBuilder> productBuilder);
    IProtocolBuilder Product(IProduct product);
    
    IProtocolBuilder Meta(Action<IMetaBuilder> metaBuilder);
    IProtocolBuilder Meta(IMeta meta);
    
    IProtocolBuilder Results(Action<IResultsBuilder> resultsBuilder);
    IProtocolBuilder Results(IResults results);
    
    IProtocol Build();
}
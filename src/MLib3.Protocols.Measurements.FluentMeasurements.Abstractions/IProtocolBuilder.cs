namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IProtocolBuilder
{
    IProtocolBuilder Product(Action<IProductBuilder> productBuilder);
    IProtocolBuilder Meta(Action<IMetaBuilder> metaBuilder);
    IProtocolBuilder Results(Action<IResultsBuilder> resultsBuilder);
    IProtocol Build();
}
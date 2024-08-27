namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class MetaBuilderFactory : IMetaBuilderFactory
{
    public MetaBuilderFactory()
    {
        
    }

    public IMetaBuilder Create()
    {
        return new MetaBuilder();
    }
}
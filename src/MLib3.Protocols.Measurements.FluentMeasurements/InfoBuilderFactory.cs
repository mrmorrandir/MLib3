namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class InfoBuilderFactory : IInfoBuilderFactory
{
    public InfoBuilderFactory()
    {
        
    }

    public IInfoBuilder Create()
    {
        return new InfoBuilder();
    }

    public IInfoBuilder Create(IInfoSetting infoSetting)
    {
        if (infoSetting == null) throw new ArgumentNullException(nameof(infoSetting));
        return new InfoBuilder(infoSetting);
    }
}
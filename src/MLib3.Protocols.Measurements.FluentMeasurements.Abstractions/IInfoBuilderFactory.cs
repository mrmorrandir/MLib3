namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IInfoBuilderFactory
{
    IInfoBuilder Create();
    IInfoBuilder Create(IInfoSetting infoSetting);
}
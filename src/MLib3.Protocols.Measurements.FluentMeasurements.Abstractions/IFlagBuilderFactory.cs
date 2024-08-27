namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IFlagBuilderFactory
{
    IFlagBuilder Create();
    IFlagBuilder Create(IFlagSetting flagSetting);
}
namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IValueBuilderFactory
{
    IValueBuilder Create();
    IValueBuilder Create(IValueSetting valueSetting);
}
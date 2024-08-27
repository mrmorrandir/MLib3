namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IValueSettingBuilderFactory
{
    IValueSettingBuilder Create();
    IValueSettingBuilder Create(IValueSetting valueSetting);
}
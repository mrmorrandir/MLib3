namespace MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

public interface IValueSettingBuilder
{
    IValueSettingBuilder Name(string name);
    IValueSettingBuilder Description(string description);
    IValueSettingBuilder Unit(string unit);
    IValueSettingBuilder Precision(double precision);
    IValueSettingBuilder Max(double max, ValueLimitType limitType = ValueLimitType.Value);
    IValueSettingBuilder Nom(double nominal);
    IValueSettingBuilder Min(double min, ValueLimitType limitType = ValueLimitType.Value);
    
    IValueSetting Build();
}
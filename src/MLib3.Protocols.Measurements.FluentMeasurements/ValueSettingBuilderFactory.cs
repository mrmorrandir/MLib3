namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ValueSettingBuilderFactory : IValueSettingBuilderFactory
{
    public ValueSettingBuilderFactory()
    {
        
    }

    public IValueSettingBuilder Create()
    {
        return new ValueSettingBuilder();
    }

    public IValueSettingBuilder Create(IValueSetting valueSetting)
    {
        if (valueSetting == null) throw new ArgumentNullException(nameof(valueSetting));
        return new ValueSettingBuilder(valueSetting);
    }
}
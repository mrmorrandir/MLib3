namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ValueBuilderFactory : IValueBuilderFactory
{
    public ValueBuilderFactory()
    {
        
    }

    public IValueBuilder Create()
    {
        return new ValueBuilder();
    }

    public IValueBuilder Create(IValueSetting valueSetting)
    {
        if (valueSetting == null) throw new ArgumentNullException(nameof(valueSetting));
        return new ValueBuilder(valueSetting);
    }
}
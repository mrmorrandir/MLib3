namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ValueSettingBuilder : IValueSettingBuilder
{
    private readonly ValueSetting _valueSetting;
    
    public ValueSettingBuilder(IValueSetting? valueSetting = null)
    {
        _valueSetting = valueSetting is null ? new ValueSetting() : new ValueSetting(valueSetting);
    }
    
    public IValueSettingBuilder Name(string name)
    {
        _valueSetting.Name = name;
        return this;
    }

    public IValueSettingBuilder Description(string description)
    {
        _valueSetting.Description = description;
        return this;
    }
   
    public IValueSettingBuilder Unit(string unit)
    {
        _valueSetting.Unit = unit;
        return this;
    }
    
    public IValueSettingBuilder Precision(double precision)
    {
        _valueSetting.Precision = precision;
        return this;
    }
    
    
    public IValueSettingBuilder Min(double min, ValueLimitType limitType = ValueLimitType.Value)
    {
        _valueSetting.Min = min;
        _valueSetting.MinLimitType = limitType;
        return this;
    }
    
    public IValueSettingBuilder Nom(double nominal)
    {
        _valueSetting.Nom = nominal;
        return this;
    }

    public IValueSettingBuilder Max(double max, ValueLimitType limitType = ValueLimitType.Value)
    {
        _valueSetting.Max = max;
        _valueSetting.MaxLimitType = limitType;
        return this;
    }

    public IValueSetting Build()
    {
        if (string.IsNullOrWhiteSpace(_valueSetting.Name))
            throw new InvalidOperationException($"{nameof(_valueSetting.Name)} is not set");
        var isNomOutOfLimits = _valueSetting.Min is not null && _valueSetting.Max is not null && _valueSetting.Nom is not null && (_valueSetting.Nom.Value < _valueSetting.Min.Value || _valueSetting.Nom.Value > _valueSetting.Max.Value);
        if (isNomOutOfLimits)
            throw new InvalidOperationException($"{nameof(_valueSetting.Nom)} {_valueSetting.Nom} is out of limits [{_valueSetting.Min}, {_valueSetting.Max}].");
        if (string.IsNullOrWhiteSpace(_valueSetting.Unit))
            throw new InvalidOperationException($"{nameof(_valueSetting.Unit)} is not set.");
        return _valueSetting;
    }
}
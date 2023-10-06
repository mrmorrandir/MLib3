namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ValueBuilder : IValueBuilder
{
    private readonly Value _value;
    private bool _isResultSet;
    private bool _isResultValueSet;

    private ValueBuilder(IValueSetting? valueSetting = null)
    {
        _value = valueSetting is null ? new Value() : new Value(valueSetting);
    }
    
    public static IValueBuilder Create()
    {
        return new ValueBuilder();
    }

    public static IValueBuilder CreateFromSetting(IValueSetting valueSetting)
    {
        if (valueSetting == null) throw new ArgumentNullException(nameof(valueSetting));
        return new ValueBuilder(valueSetting);
    }

    public IValueBuilder Name(string name)
    {
        _value.Name = name;
        return this;
    }

    public IValueBuilder Description(string description)
    {
        _value.Description = description;
        return this;
    }
   
    public IValueBuilder Unit(string unit)
    {
        _value.Unit = unit;
        return this;
    }
    
    public IValueBuilder Precision(double precision)
    {
        _value.Precision = precision;
        return this;
    }
    
    public IValueBuilder Min(double min, ValueLimitType limitType = ValueLimitType.Value)
    {
        _value.Min = min;
        _value.MinLimitType = limitType;
        return this;
    }
    
    public IValueBuilder Nom(double nominal)
    {
        _value.Nom = nominal;
        return this;
    }

    public IValueBuilder Max(double max, ValueLimitType limitType = ValueLimitType.Value)
    {
        _value.Max = max;
        _value.MaxLimitType = limitType;
        return this;
    }
    
    public IValueBuilder Result(double value)
    {
        _value.Result = value;
        _isResultValueSet = true;
        return this;
    }
    
    public IValueBuilder OK()
    {
        _value.OK = true;
        _isResultSet = true;
        return this;
    }

    public IValueBuilder NOK()
    {
        _value.OK = false;
        _isResultSet = true;
        return this;
    }
    
    public IValueBuilder Evaluate()
    {
        var isMinimumInTolerance = _value.Min is null || _value.Min.Value <= _value.Result;
        var isMaximumInTolerance = _value.Max is null || _value.Max.Value >= _value.Result;
            
        _value.OK = isMinimumInTolerance && isMaximumInTolerance;
        _isResultSet = true;
        return this;
    }

    public IValue Build()
    {
        if (string.IsNullOrWhiteSpace(_value.Name))
            throw new InvalidOperationException($"{nameof(_value.Name)} is not set");
        var isNomOutOfLimits = _value.Min is not null && _value.Max is not null && _value.Nom is not null && (_value.Nom.Value < _value.Min.Value || _value.Nom.Value > _value.Max.Value);
        if (isNomOutOfLimits)
            throw new InvalidOperationException($"{nameof(_value.Nom)} {_value.Nom} is out of limits [{_value.Min}, {_value.Max}].");
        if (!_isResultValueSet)
            throw new InvalidOperationException($"{nameof(_value.Result)} is not set");
        if (!_isResultSet)
            throw new InvalidOperationException($"Result is not set. Use {nameof(OK)}, {nameof(NOK)} or {nameof(Evaluate)}");
        if (string.IsNullOrWhiteSpace(_value.Unit))
            throw new InvalidOperationException($"{nameof(_value.Unit)} is not set.");
        return _value;
    }
}
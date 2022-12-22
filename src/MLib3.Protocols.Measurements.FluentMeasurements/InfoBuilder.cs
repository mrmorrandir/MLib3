using MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class InfoBuilder : IInfoBuilder
{
    private readonly Info _info;
    private bool _isValueSet;

    private InfoBuilder(IInfoSetting? infoSetting = null)
    {
        _info = infoSetting is null ? new Info() : new Info(infoSetting);
    }

    public static IInfoBuilder Create()
    {
        return new InfoBuilder();
    }
    
    public static IInfoBuilder CreateFromSetting(IInfoSetting infoSetting)
    {
        if (infoSetting == null) throw new ArgumentNullException(nameof(infoSetting));
        return new InfoBuilder(infoSetting);
    }
    
    public IInfoBuilder Name(string name)
    {
        _info.Name = name;
        return this;
    }
    
    public IInfoBuilder Description(string description)
    {
        _info.Description = description;
        return this;
    }
    
    public IInfoBuilder Unit(string unit)
    {
        _info.Unit = unit;
        return this;
    }
    
    public IInfoBuilder Precision(double precision)
    {
        _info.Precision = precision;
        return this;
    }
    
    public IInfoBuilder Value(double value)
    {
        _info.Value = value;
        _isValueSet = true;
        return this;
    }
    
    public IInfo Build()
    {
        if (string.IsNullOrWhiteSpace(_info.Name))
            throw new InvalidOperationException($"{nameof(_info.Name)} is not set");
        if (string.IsNullOrWhiteSpace(_info.Unit))
            throw new InvalidOperationException($"{nameof(_info.Unit)} is not set.");
        if (!_isValueSet)
            throw new InvalidOperationException($"{nameof(_info.Value)} is not set.");
        return _info;
    }
}
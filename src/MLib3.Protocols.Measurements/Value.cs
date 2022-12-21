namespace MLib3.Protocols.Measurements;

public class Value : IValue
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public double? Precision { get; set; }
    public double? Min { get; set; }
    public double? Nom { get; set; }
    public double? Max { get; set; }
    public ValueLimitType? MinLimitType { get; set; }
    public ValueLimitType? MaxLimitType { get; set; }
    public double Result { get; set; }
    public bool OK { get; set; }

    public Value() { }

    public Value(IValueSetting valueSetting, double result = 0.0, bool ok = false)
    {
        if (valueSetting == null) throw new ArgumentNullException(nameof(valueSetting));
        Name = valueSetting.Name;
        Description = valueSetting.Description;
        Unit = valueSetting.Unit;
        Precision = valueSetting.Precision;
        Min = valueSetting.Min;
        Nom = valueSetting.Nom;
        Max = valueSetting.Max;
        MinLimitType = valueSetting.MinLimitType;
        MaxLimitType = valueSetting.MaxLimitType;
        Result = result;
        OK = ok;
        Extensions = null;
    }
}
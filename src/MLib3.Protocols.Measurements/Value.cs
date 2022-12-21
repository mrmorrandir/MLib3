namespace MLib3.Protocols.Measurements;

public class Value : IValue
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Precision { get; set; }
    public string? Unit { get; set; }
    public double? Maximum { get; set; }
    public double? Nominal { get; set; }
    public double? Minimum { get; set; }
    public ValueLimitType? MinimumLimitType { get; set; }
    public ValueLimitType? MaximumLimitType { get; set; }
    public bool OK { get; set; }
    public double Result { get; set; }

    public Value() { }

    public Value(IValueSetting valueSetting, double result = 0.0, bool ok = false)
    {
        if (valueSetting == null) throw new ArgumentNullException(nameof(valueSetting));
        Name = valueSetting.Name;
        Description = valueSetting.Description;
        Precision = valueSetting.Precision;
        Unit = valueSetting.Unit;
        Maximum = valueSetting.Maximum;
        Nominal = valueSetting.Nominal;
        Minimum = valueSetting.Minimum;
        MinimumLimitType = valueSetting.MinimumLimitType;
        MaximumLimitType = valueSetting.MaximumLimitType;
        Result = result;
        OK = ok;
    }
}
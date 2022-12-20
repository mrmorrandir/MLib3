namespace MLib3.Protocols.Measurements;

public class Value : IValue
{
    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; init; }
    public double? Precision { get; init; }
    public string Unit { get; init; }
    public double? Maximum { get; init; }
    public double? Nominal { get; init; }
    public double? Minimum { get; init; }
    public ValueLimitType? MinimumLimitType { get; init; }
    public ValueLimitType? MaximumLimitType { get; init; }
    public bool OK { get; set; }
    public double Result { get; set; }
    public Value() { }

    public Value(IValueSetting valueSetting, double result = 0.0, bool ok = false)
    {
        if (valueSetting == null) throw new ArgumentNullException(nameof(valueSetting));
        Name = valueSetting.Name;
        Hint = valueSetting.Hint;
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
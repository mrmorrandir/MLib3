namespace MLib3.Protocols.Measurements;

public class ValueSetting : IValueSetting
{
    public ValueSetting(string name, string unit, string? hint = null, double? precision = null, double? minimum = null, double? nominal = null,
        double? maximum = null, ValueLimitType? minimumLimitType = null, ValueLimitType? maximumLimitType = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(unit));
        Name = name;
        Hint = hint;
        Precision = precision;
        Unit = unit;
        Maximum = maximum;
        Nominal = nominal;
        Minimum = minimum;
        MinimumLimitType = minimumLimitType;
        MaximumLimitType = maximumLimitType;
    }
    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; }
    public double? Precision { get; }
    public string Unit { get; }
    public double? Maximum { get; }
    public double? Nominal { get; }
    public double? Minimum { get; }
    public ValueLimitType? MinimumLimitType { get; }
    public ValueLimitType? MaximumLimitType { get; }
}
namespace MLib3.Protocols.Measurements;

public class ValueSetting : IValueSetting
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

    public ValueSetting(string? name, string? unit, string? hint = null, double? precision = null, double? minimum = null, double? nominal = null,
        double? maximum = null, ValueLimitType? minimumLimitType = null, ValueLimitType? maximumLimitType = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(unit));
        Name = name;
        Description = hint;
        Precision = precision;
        Unit = unit;
        Maximum = maximum;
        Nominal = nominal;
        Minimum = minimum;
        MinimumLimitType = minimumLimitType;
        MaximumLimitType = maximumLimitType;
    }
}
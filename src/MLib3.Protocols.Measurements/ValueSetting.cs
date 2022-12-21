namespace MLib3.Protocols.Measurements;

public class ValueSetting : IValueSetting
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

    public ValueSetting() { }

    /// <summary>
    ///     Creates a new instance of the <see cref="ValueSetting" /> class.
    ///     <para>All values are copied from the given <paramref name="valueSetting" />, except for the Extensions.</para>
    /// </summary>
    /// <param name="valueSetting">The <see cref="ValueSetting" /> to copy from</param>
    public ValueSetting(IValueSetting valueSetting)
    {
        Name = valueSetting.Name;
        Description = valueSetting.Description;
        Unit = valueSetting.Unit;
        Precision = valueSetting.Precision;
        Min = valueSetting.Min;
        Nom = valueSetting.Nom;
        Max = valueSetting.Max;
        MinLimitType = valueSetting.MinLimitType;
        MaxLimitType = valueSetting.MaxLimitType;
        Extensions = null;
    }

    public ValueSetting(string name, string? description = null, string? unit = null, double? precision = null, double? minimum = null, double? nominal = null,
        double? maximum = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Precision = precision;
        Unit = unit;
        Max = maximum;
        Nom = nominal;
        Min = minimum;
        MinLimitType = minLimitType;
        MaxLimitType = maxLimitType;
        Extensions = null;
    }
}
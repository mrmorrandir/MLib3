namespace MLib3.Protocols.Measurements;

public class InfoSetting : IInfoSetting
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Precision { get; set; }
    public string Unit { get; set; }

    public InfoSetting() { }

    public InfoSetting(string? name, string unit, string? hint = null, double? precision = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(unit));
        Name = name;
        Unit = unit;
        Description = hint;
        Precision = precision;
    }
}
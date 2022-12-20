namespace MLib3.Protocols.Measurements;

public class InfoSetting : IInfoSetting
{
    public InfoSetting(string name, string unit, string? hint = null, double? precision = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(unit));
        Name = name;
        Unit = unit;
        Hint = hint;
        Precision = precision;
    }
    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; }
    public double? Precision { get; }
    public string Unit { get; }
}
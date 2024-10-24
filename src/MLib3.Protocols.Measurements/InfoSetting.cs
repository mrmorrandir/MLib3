namespace MLib3.Protocols.Measurements;

public class InfoSetting : IInfoSetting
{
    public IExtensions? Extensions { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double? Precision { get; set; }
    public string Unit { get; set; } = string.Empty;

    public InfoSetting() { }

    public InfoSetting(string name, string? description = null, string? unit = null, double? precision = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Unit = unit ?? string.Empty;
        Precision = precision;
        Extensions = null;
    }
}
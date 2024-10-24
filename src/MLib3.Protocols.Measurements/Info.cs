namespace MLib3.Protocols.Measurements;

public class Info : IInfo
{
    public IExtensions? Extensions { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double? Precision { get; set; }
    public string Unit { get; set; } = string.Empty;
    public double Value { get; set; } = 0;

    public Info() { }

    public Info(IInfoSetting infoSetting, double value = 0)
    {
        if (infoSetting == null) throw new ArgumentNullException(nameof(infoSetting));
        Name = infoSetting.Name;
        Description = infoSetting.Description;
        Precision = infoSetting.Precision;
        Unit = infoSetting.Unit;
        Value = value;
        Extensions = null;
    }
}
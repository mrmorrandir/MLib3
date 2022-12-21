namespace MLib3.Protocols.Measurements;

public class Info : IInfo
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Precision { get; set; }
    public string Unit { get; set; }
    public double Value { get; set; }

    public Info() { }

    public Info(IInfoSetting infoSetting, double value = 0)
    {
        if (infoSetting == null) throw new ArgumentNullException(nameof(infoSetting));
        Name = infoSetting.Name;
        Description = infoSetting.Description;
        Precision = infoSetting.Precision;
        Unit = infoSetting.Unit;
        Value = value;
    }
}
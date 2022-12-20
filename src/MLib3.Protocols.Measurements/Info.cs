namespace MLib3.Protocols.Measurements;

public class Info : IInfo
{
    public Info(IInfoSetting infoSetting, double value = 0)
    {
        if (infoSetting == null) throw new ArgumentNullException(nameof(infoSetting));
        Name = infoSetting.Name;
        Hint = infoSetting.Hint;
        Precision = infoSetting.Precision;
        Unit = infoSetting.Unit;
        Value = value;
    }
    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; }
    public double? Precision { get; }
    public string Unit { get; }
    public double Value { get; set; }
}
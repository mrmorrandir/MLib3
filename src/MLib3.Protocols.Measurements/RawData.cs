namespace MLib3.Protocols.Measurements;

public class RawData : IRawData
{
    public IExtensions? Extensions { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Format { get; set; }
    public string Raw { get; set; } = string.Empty;
    public RawData() { }
    
    public RawData(IRawDataSetting rawDataSetting, string raw = "")
    {
        if (rawDataSetting == null) throw new ArgumentNullException(nameof(rawDataSetting));
        Name = rawDataSetting.Name;
        Description = rawDataSetting.Description;
        Format = rawDataSetting.Format;
        Raw = raw;
        Extensions = null;
    }
}
namespace MLib3.Protocols.Measurements;

public class RawDataSetting : IRawDataSetting
{
    public IExtensions? Extensions { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Format { get; set; }
    
    public RawDataSetting() { }
    
    public RawDataSetting(string name, string? description = null, string? format = null)
    {
        Name = name;
        Description = description;
        Format = format;
        Extensions = null;
    }
}
namespace MLib3.Protocols.Measurements;

public class FlagSetting : IFlagSetting
{
    public IExtensions? Extensions { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FlagSetting() { }

    public FlagSetting(string name, string? description = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Extensions = null;
    }
}
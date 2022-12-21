namespace MLib3.Protocols.Measurements;

public class SectionSetting : ISectionSetting
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public SectionSetting() { }

    public SectionSetting(string? name, string? hint = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        Name = name;
        Description = hint;
    }
}
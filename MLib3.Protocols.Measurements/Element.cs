namespace MLib3.Protocols.Measurements;

public abstract class Element
{
    [XmlAttribute]
    public string Name { get; set; } = string.Empty;
    [XmlAttribute]
    public string? Description { get; set; }
    
    public Extensions? Extensions { get; set; }

    protected Element() {}

    protected Element(string name, string? description = null, Extensions? extensions = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Extensions = extensions;
    }
}
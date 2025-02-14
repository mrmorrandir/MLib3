using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public abstract class Element
{
    [XmlAttribute]
    public string Name { get; set; } = string.Empty;
    [XmlAttribute]
    public string? Description { get; set; }
    
    public Extensions? Extensions { get; set; }

    protected Element() {}

    protected Element(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}
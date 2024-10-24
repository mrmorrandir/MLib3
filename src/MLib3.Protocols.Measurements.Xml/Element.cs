using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Xml;

public abstract class Element
{
    [XmlAttribute]
    public string Name { get; set; } = string.Empty;
    [XmlAttribute]
    public string? Description { get; set; }
    
    public virtual Extensions? Extensions { get; set; }
}
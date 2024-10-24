using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Xml;

public class Protocol
{
    public Product Product { get; set; } = new();
    public Meta Meta { get; set; } = new();
    public Results Results { get; set; } = new();

    [XmlAttribute]
    public string Specification { get; set; } = string.Empty;

    [XmlAttribute]
    public string Version { get; set; } = string.Empty;
}
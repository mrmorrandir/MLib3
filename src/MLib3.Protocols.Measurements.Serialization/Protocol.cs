using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Protocol
{
    public Product Product { get; set; } = new();
    public Meta Meta { get; set; } = new();
    public Results Results { get; set; } = new();

    [XmlAttribute]
    public string Specification { get; set; } = string.Empty;

    [XmlAttribute]
    public string Version { get; set; } = string.Empty;

    public Protocol()
    {
    }

    public Protocol(Product product, Meta meta, Results results)
    {
        Product = product;
        Meta = meta;
        Results = results;
    }
}
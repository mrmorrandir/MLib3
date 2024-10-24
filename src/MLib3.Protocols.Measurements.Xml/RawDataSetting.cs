using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Xml;

public class RawDataSetting : Element
{
    [XmlAttribute]
    public string? Format { get; set; }
}
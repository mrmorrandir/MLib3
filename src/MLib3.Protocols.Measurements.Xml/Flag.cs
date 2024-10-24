using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Xml;

public class Flag : FlagSetting
{
    [XmlAttribute]
    public bool Ok { get; set; }
}
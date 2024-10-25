using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Flag : FlagSetting
{
    [XmlAttribute]
    public bool Ok { get; set; }
}
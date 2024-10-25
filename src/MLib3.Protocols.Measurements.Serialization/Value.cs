using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Value : ValueSetting
{
    public double Result { get; set; }
    [XmlAttribute]
    public bool Ok { get; set; }
}
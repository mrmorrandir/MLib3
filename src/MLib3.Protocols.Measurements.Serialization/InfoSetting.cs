using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class InfoSetting : Element
{
    [XmlAttribute]
    public double Precision { get; set; }

    [XmlIgnore]
    public bool PrecisionSpecified { get; set; }

    [XmlAttribute]
    public string Unit { get; set; } = string.Empty;
}
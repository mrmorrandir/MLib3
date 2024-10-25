using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class ValueSetting : Element
{
    [XmlAttribute]
    public string Unit { get; set; } = string.Empty;
    [XmlAttribute]
    public double Precision { get; set; }
    [XmlIgnore]
    public bool PrecisionSpecified { get; set; }
    public double? Min { get; set; }
    public double? Nom { get; set; }
    public double? Max { get; set; }
    public ValueLimitType? MinLimitType { get; set; }
    public ValueLimitType? MaxLimitType { get; set; }
}
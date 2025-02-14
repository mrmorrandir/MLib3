using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class InfoSetting : Element
{
    [XmlAttribute]
    public double Precision { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public bool PrecisionSpecified { get; set; }

    [XmlAttribute]
    public string Unit { get; set; } = string.Empty;
    
    public InfoSetting() {}

    public InfoSetting(string name, string? description = null, double? precision = 0.0, string? unit = null) : base(name, description)
    {
        Precision = precision ?? 0.0;
        Unit = unit ?? string.Empty;
    }
}
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class ValueSetting : Element
{
    [XmlAttribute]
    public string Unit { get; set; } = string.Empty;
    [XmlAttribute]
    public double Precision { get; set; }
    [XmlIgnore]
    [JsonIgnore]
    public bool PrecisionSpecified { get; set; }
    public double? Min { get; set; }
    public double? Nom { get; set; }
    public double? Max { get; set; }
    public ValueLimitType? MinLimitType { get; set; }
    public ValueLimitType? MaxLimitType { get; set; }
    
    public ValueSetting() {}
    
    public ValueSetting(string name, string? description = null, string? unit = null, double? precision = 0.0, double? min = null, double? nom = null, double? max = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null) : base(name, description)
    {
        Unit = unit ?? string.Empty;
        Precision = precision ?? 0.0;
        Min = min;
        Nom = nom;
        Max = max;
        MinLimitType = minLimitType;
        MaxLimitType = maxLimitType;
    }
}
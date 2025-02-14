using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Value : ValueSetting
{
    public double Result { get; set; }
    [XmlAttribute]
    public bool Ok { get; set; }
    
    public Value() {}
    
    public Value(string name, string? description = null, string? unit = null, double? precision = 0.0, double? min = null, double? nom = null, double? max = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null, double? result = 0.0, bool? ok = false) : base(name, description, unit, precision, min, nom, max, minLimitType, maxLimitType)
    {
        Result = result ?? 0.0;
        Ok = ok ?? false;
    }
}
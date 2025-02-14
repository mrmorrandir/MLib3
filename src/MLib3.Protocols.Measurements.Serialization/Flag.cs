using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Flag : FlagSetting
{
    [XmlAttribute]
    public bool Ok { get; set; }
    
    public Flag() {}
    
    public Flag(string name, string? description = null, bool? ok = false) : base(name, description)
    {
        Ok = ok ?? false;
    }
}
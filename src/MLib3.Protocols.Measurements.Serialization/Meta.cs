using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Meta
{
    [XmlAttribute]
    public string Type { get; set; } = string.Empty;
    
    public DateTime Timestamp { get; set; }
    public string? DeviceName { get; set; }
    public string? DeviceId { get; set; }
    public string? Operator { get; set; }
    public string? Software { get; set; }
    public string? SoftwareVersion { get; set; }
    public string? TestRoutine { get; set; }
    public string? TestRoutineVersion { get; set; }
    public Extensions? Extensions { get; set; }
}
namespace MLib3.Protocols.Measurements.Xml;

public class Meta
{
    public Extensions? Extensions { get; set; }
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string? Software { get; set; }
    public string? SoftwareVersion { get; set; }
    public string? TestRoutine { get; set; }
    public string? TestRoutineVersion { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Operator { get; set; }
}
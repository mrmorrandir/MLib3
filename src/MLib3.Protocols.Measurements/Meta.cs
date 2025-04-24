using System.Reflection;

namespace MLib3.Protocols.Measurements;

public class Meta
{
    [XmlAttribute]
    public string Type { get; set; } = "Unknown";

    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string? DeviceName { get; set; } = Environment.MachineName;
    public string? DeviceId { get; set; } = Environment.MachineName;
    public string? Operator { get; set; } = Environment.UserName;
    public string? Software { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
    public string? SoftwareVersion { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version.ToString(3) ?? "Unknown";
    public string? TestRoutine { get; set; }
    public string? TestRoutineVersion { get; set; }
    public Extensions? Extensions { get; set; }
    
    public Meta() {}

    public Meta(string? type = null, DateTime? timestamp = null, string? deviceName = null, string? deviceId = null, string? operatorName = null, string? software = null, string? softwareVersion = null, string? testRoutine = null, string? testRoutineVersion = null, Extensions? extensions = null)
    {
        Type = type ?? "Unknown";
        Timestamp = timestamp ?? DateTime.Now;
        DeviceName = deviceName ?? Environment.MachineName;
        DeviceId = deviceId ?? Environment.MachineName;
        Operator = operatorName ?? Environment.UserName;
        Software = software ?? Assembly.GetEntryAssembly()?.FullName ?? "Unknown";
        SoftwareVersion = softwareVersion ?? Assembly.GetEntryAssembly()?.GetName().Version.ToString(3) ?? "Unknown";;
        TestRoutine = testRoutine;
        TestRoutineVersion = testRoutineVersion;
        Extensions = extensions;
    }
    
    public override string ToString() => FormattableString.Invariant($"Meta: {Type}, Timestamp: {Timestamp}, DeviceName: {DeviceName}, DeviceId: {DeviceId}, Operator: {Operator}, Software: {Software}, SoftwareVersion: {SoftwareVersion}, TestRoutine: {TestRoutine}, TestRoutineVersion: {TestRoutineVersion}");
}
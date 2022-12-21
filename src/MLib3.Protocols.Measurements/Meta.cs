using System.Reflection;

namespace MLib3.Protocols.Measurements;

public class Meta : IMeta
{
    public IExtensions? Extensions { get; set; }
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string? Software { get; set; }
    public string? SoftwareVersion { get; set; }
    public string? TestRoutine { get; set; }
    public string? TestRoutineVersion { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; }
    public string? Operator { get; set; }

    public Meta()
    {
        Timestamp = DateTime.Now;
        Type = null!;
        DeviceId = Environment.MachineName;
        DeviceName = Environment.MachineName;
        Software = Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
        SoftwareVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
        TestRoutine = null!;
        TestRoutineVersion = null!;
        Operator = Environment.UserName;
        Extensions = null;
    }

    public Meta(string type, DateTime timestamp, string? deviceId = null, string? deviceName = null, string? software = null, string? softwareVersion = null, string? testRoutine = null, string? testRoutineVersion = null, string? @operator = null)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(type));
        if (DateTime.Now < timestamp)
            throw new ArgumentException("Value cannot be in the future.", nameof(timestamp));
        if (software is null && softwareVersion is not null)
            throw new ArgumentException($"Value of {nameof(softwareVersion)} cannot be used without a {nameof(software)}.", nameof(softwareVersion));
        if (softwareVersion is null && software is not null)
            throw new ArgumentException($"Value of {nameof(software)} cannot be used without a {nameof(softwareVersion)}.", nameof(software));
        if (testRoutine is null && testRoutineVersion is not null)
            throw new ArgumentException($"Value of {nameof(testRoutineVersion)} cannot be used without a {nameof(testRoutine)}.", nameof(testRoutineVersion));
        if (testRoutineVersion is null && testRoutine is not null)
            throw new ArgumentException($"Value of {nameof(testRoutine)} cannot be used without a {nameof(testRoutineVersion)}.", nameof(testRoutine));
        Type = type;
        Timestamp = timestamp;
        DeviceId = deviceId ?? Environment.MachineName;
        DeviceName = deviceName ?? Environment.MachineName;
        Software = software ?? Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
        SoftwareVersion = softwareVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
        TestRoutine = testRoutine;
        TestRoutineVersion = testRoutineVersion;
        Operator = @operator;
        Extensions = null;
    }
}
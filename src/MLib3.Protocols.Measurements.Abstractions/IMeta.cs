namespace MLib3.Protocols.Measurements.Abstractions;

public interface IMeta : IExtendable
{
    string? DeviceId { get; }
    string? DeviceName { get; }
    string? Software { get; }
    string? SoftwareVersion { get; }
    string? TestRoutine { get; }
    string? TestRoutineVersion { get; }
    DateTime Timestamp { get; }
    string Type { get; }
    string? Operator { get; }
}
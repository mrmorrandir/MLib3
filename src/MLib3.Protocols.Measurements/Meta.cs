using System.Reflection;
using System.Text.RegularExpressions;

namespace MLib3.Protocols.Measurements;

public class Meta : IMeta
{
    private static readonly Regex _versionRegex =
        new(@"^((?<Major>\d*)|(?<MajorMinor>(\d*\.\d*))|(?<Semantic>(\d*\.\d*\.\d*))|(?<AssemblyVersion>(\d*\.\d*\.\d*\.\d*))|(?<OldSchool>[vV]\d*))$",
            RegexOptions.Compiled);

    public IExtensions? Extensions { get; set; }
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string? Program { get; set; }
    public string? ProgramVersion { get; set; }
    public string? TestRoutine { get; set; }
    public string? TestRoutineVersion { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; }
    public string? Worker { get; set; }

    public Meta()
    {
        Timestamp = DateTime.Now;
        Type = null!;
        DeviceId = Environment.MachineName;
        DeviceName = Environment.MachineName;
        Program = Assembly.GetEntryAssembly()?.GetName().FullName ?? string.Empty;
        ProgramVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
        TestRoutine = null!;
        TestRoutineVersion = null!;
        Worker = Environment.UserName;
    }

    public Meta(string type, DateTime timestamp, string? deviceId = null, string? deviceName = null, string? program = null, string? programVersion = null, string? testRoutine = null, string? testRoutineVersion = null, string? worker = null)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(type));
        if (DateTime.Now < timestamp)
            throw new ArgumentException("Value cannot be in the future.", nameof(timestamp));
        if (programVersion != null && !_versionRegex.IsMatch(programVersion))
            throw new ArgumentException("Value is in the wrong format. (Allowed Formats '1', '1.0', '1.0.0', 'v1', 'V1')", nameof(programVersion));
        if (testRoutineVersion != null && !_versionRegex.IsMatch(testRoutineVersion))
            throw new ArgumentException("Value is in the wrong format. (Allowed Formats '1', '1.0', '1.0.0', 'v1', 'V1')", nameof(testRoutineVersion));
        if (program is null && programVersion is not null)
            throw new ArgumentException($"Value of {nameof(programVersion)} cannot be used without a {nameof(program)}.", nameof(programVersion));
        if (testRoutine is null && testRoutineVersion is not null)
            throw new ArgumentException($"Value of {nameof(testRoutineVersion)} cannot be used without a {nameof(testRoutine)}.", nameof(testRoutineVersion));
        Type = type;
        Timestamp = timestamp;
        DeviceId = deviceId ?? Environment.MachineName;
        DeviceName = deviceName ?? Environment.MachineName;
        Program = program ?? Assembly.GetEntryAssembly()?.GetName().FullName ?? string.Empty;
        ProgramVersion = programVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
        TestRoutine = testRoutine;
        TestRoutineVersion = testRoutineVersion;
        Worker = worker;
    }
}
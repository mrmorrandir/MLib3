using MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class MetaBuilder : IMetaBuilder
{
    private readonly Meta _meta;
    private readonly List<Action> _builders = new();
    
    private MetaBuilder()
    {
        _meta = new Meta();
    }
    
    public static IMetaBuilder Create()
    {
        return new MetaBuilder();
    }

    public IMetaBuilder DeviceId(string deviceId)
    {
        _builders.Add(() => _meta.DeviceId = deviceId);
        return this;
    }

    public IMetaBuilder DeviceName(string deviceName)
    {
        _builders.Add(() => _meta.DeviceName = deviceName);
        return this;
    }

    public IMetaBuilder Software(string software)
    {
        _builders.Add(() => _meta.Software = software);
        return this;
    }

    public IMetaBuilder SoftwareVersion(string softwareVersion)
    {
        _builders.Add(() => _meta.SoftwareVersion = softwareVersion);
        return this;
    }

    public IMetaBuilder TestRoutine(string testRoutine)
    {
        _builders.Add(() => _meta.TestRoutine = testRoutine);
        return this;
    }

    public IMetaBuilder TestRoutineVersion(string testRoutineVersion)
    {
        _builders.Add(() => _meta.TestRoutineVersion = testRoutineVersion);
        return this;
    }

    public IMetaBuilder Timestamp(DateTime timestamp)
    {
        _builders.Add(() => _meta.Timestamp = timestamp);
        return this;
    }

    public IMetaBuilder Type(string type)
    {
        _builders.Add(() => _meta.Type = type);
        return this;
    }

    public IMetaBuilder Operator(string @operator)
    {
        _builders.Add(() => _meta.Operator = @operator);
        return this;
    }

    public IMeta Build()
    {
        foreach (var builder in _builders)
            builder();
        if (string.IsNullOrWhiteSpace(_meta.Type))
            throw new InvalidOperationException($"{nameof(_meta.Type)} is not set.");
        if (_meta.Timestamp == default)
            throw new InvalidOperationException($"{nameof(_meta.Timestamp)} is not set.");
        return _meta;
    }
}
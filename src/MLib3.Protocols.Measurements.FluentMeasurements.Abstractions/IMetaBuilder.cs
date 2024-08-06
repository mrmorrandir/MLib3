namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IMetaBuilder
{
    IMetaBuilder DeviceId(string deviceId) ;
    IMetaBuilder DeviceName(string deviceName) ;
    IMetaBuilder Software(string software) ;
    IMetaBuilder SoftwareVersion(string softwareVersion) ;
    IMetaBuilder TestRoutine(string testRoutine) ;
    IMetaBuilder TestRoutineVersion(string testRoutineVersion) ;
    IMetaBuilder Timestamp(DateTime timestamp) ;
    IMetaBuilder Type(string type) ;
    IMetaBuilder Operator(string @operator) ;

    IMeta Build();
}
namespace MLib3.Protocols.Measurements.Serialization.Mappers;

public interface ISerializationMapper
{
    Serialization.Protocol Map(IProtocol protocol);
}
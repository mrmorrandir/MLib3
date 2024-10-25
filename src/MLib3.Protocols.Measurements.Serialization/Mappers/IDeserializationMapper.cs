namespace MLib3.Protocols.Measurements.Serialization.Mappers;

public interface IDeserializationMapper
{
    IProtocol Map(Serialization.Protocol protocol);
}
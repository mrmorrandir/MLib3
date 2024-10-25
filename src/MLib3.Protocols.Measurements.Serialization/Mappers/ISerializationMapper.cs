using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization.Mappers;

public interface ISerializationMapper
{
    Result<Protocol> Map(IProtocol protocol);
}
using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization.Mappers;

public interface IDeserializationMapper
{
    Result<IProtocol> Map(Protocol protocol);
}
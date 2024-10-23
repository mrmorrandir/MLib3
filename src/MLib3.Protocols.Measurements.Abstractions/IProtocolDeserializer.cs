using FluentResults;

namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProtocolDeserializer
{
    Result<IProtocol> Deserialize(string json);
}
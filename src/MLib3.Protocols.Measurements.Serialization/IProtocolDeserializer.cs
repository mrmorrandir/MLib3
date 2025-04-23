using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization;

public interface IProtocolDeserializer
{
    Result<Protocol> Deserialize(string data);
}
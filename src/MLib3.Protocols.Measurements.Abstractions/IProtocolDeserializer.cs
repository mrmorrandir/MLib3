using FluentResults;

namespace MLib3.Protocols.Measurements;

public interface IProtocolDeserializer
{
    Result<IProtocol> Deserialize(string data);
}
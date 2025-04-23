using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization;

public interface IProtocolSerializer
{
    Result<string> Serialize(Protocol protocol);
}
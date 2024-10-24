using FluentResults;

namespace MLib3.Protocols.Measurements;

public interface IProtocolSerializer
{
    Result<string> Serialize(IProtocol protocol);
}
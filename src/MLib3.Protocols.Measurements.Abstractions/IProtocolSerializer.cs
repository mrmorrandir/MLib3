using FluentResults;

namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProtocolSerializer
{
    Result<string> Serialize(IProtocol protocol);
}
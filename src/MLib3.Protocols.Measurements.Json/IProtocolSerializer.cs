using FluentResults;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Json;

public interface IProtocolSerializer
{
    Result<string> Serialize(IProtocol protocol);
}
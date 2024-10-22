using FluentResults;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Json;

public interface IProtocolDeserializer
{
    Result<IProtocol> Deserialize(string json);
}
using System.Text.Json;
using FluentResults;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Json;

public class ProtocolSerializer : IProtocolSerializer
{
    public Result<string> Serialize(IProtocol protocol)
    {
        return Result.Try(() => JsonSerializer.Serialize(protocol, ProtocolSerializerOptions.Default));
    }
}


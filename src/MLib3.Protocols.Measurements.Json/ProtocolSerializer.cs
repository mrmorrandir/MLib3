using FluentResults;
using MLib3.Protocols.Measurements.Abstractions;
using Newtonsoft.Json;

namespace MLib3.Protocols.Measurements.Json;

public class ProtocolSerializer : IProtocolSerializer
{
    public Result<string> Serialize(IProtocol protocol)
    {
        return Result.Try(() => JsonConvert.SerializeObject(protocol, ProtocolSerializerSettings.Default));
    }
}


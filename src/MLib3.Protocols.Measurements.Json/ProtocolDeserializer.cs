using System.Text.Json;
using FluentResults;
using MLib3.Protocols.Measurements.Abstractions;
using Newtonsoft.Json;

namespace MLib3.Protocols.Measurements.Json;

public class ProtocolDeserializer : IProtocolDeserializer
{
    public Result<IProtocol> Deserialize(string json)
    {
        var deserializeResult = Result.Try(() => JsonConvert.DeserializeObject<Protocol>(json, ProtocolSerializerSettings.Default));
        if (deserializeResult.IsFailed)
            return Result.Fail<IProtocol>(deserializeResult.Errors);
        
        if (deserializeResult.Value == null)
            return Result.Fail<IProtocol>("Cannot deserialize protocol.");
        
        return Result.Ok<IProtocol>(deserializeResult.Value);
    }
}
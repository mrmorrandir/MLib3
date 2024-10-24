using System.Text.Json;
using FluentResults;

namespace MLib3.Protocols.Measurements.Json;

public class ProtocolDeserializer : IProtocolDeserializer
{
    public Result<IProtocol> Deserialize(string data)
    {
        var deserializeResult = Result.Try(() => JsonSerializer.Deserialize<Protocol>(data, ProtocolSerializerOptions.Default));
        if (deserializeResult.IsFailed)
            return Result.Fail<IProtocol>(deserializeResult.Errors);
        
        if (deserializeResult.Value == null)
            return Result.Fail<IProtocol>("Cannot deserialize protocol.");
        
        return Result.Ok<IProtocol>(deserializeResult.Value);
    }
}
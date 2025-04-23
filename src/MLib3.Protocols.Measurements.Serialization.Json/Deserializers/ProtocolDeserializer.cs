using MLib3.Protocols.Measurements.Serialization.Json.Common;

namespace MLib3.Protocols.Measurements.Serialization.Json.Deserializers;

public class ProtocolDeserializer : IProtocolDeserializer
{
    public ProtocolDeserializer()
    {
    }
    public Result<Protocol> Deserialize(string json)
    {
        var deserializeResult = Result.Try(() => JsonSerializer.Deserialize<Protocol>(json, ProtocolSerializerOptions.Default));
        if (deserializeResult.IsFailed)
            return new Error("Deserialization failed").CausedBy(deserializeResult.Errors);
        if (deserializeResult.Value is null) 
            return new Error("Deserialization failed").CausedBy(new Exception("Deserialized value is null"));
        return Result.Ok(deserializeResult.Value);
    }
}
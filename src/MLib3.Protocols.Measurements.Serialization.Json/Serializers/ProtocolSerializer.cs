using MLib3.Protocols.Measurements.Serialization.Json.Common;

namespace MLib3.Protocols.Measurements.Serialization.Json.Serializers;

public class ProtocolSerializer : IProtocolSerializer
{
    public ProtocolSerializer()
    {
    }
    
    public Result<string> Serialize(Protocol protocol)
    {
        return Result.Try(() => JsonSerializer.Serialize(protocol, ProtocolSerializerOptions.Default));
    }
}
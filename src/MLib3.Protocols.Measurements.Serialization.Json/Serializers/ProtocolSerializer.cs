using System.Text.Json;
using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Json.Common;
using MLib3.Protocols.Measurements.Serialization.Mappers;

namespace MLib3.Protocols.Measurements.Serialization.Json.Serializers;


public class ProtocolSerializer : IProtocolSerializer
{
    private readonly ISerializationMapper _serializationMapper;

    public ProtocolSerializer(ISerializationMapper serializationMapper)
    {
        _serializationMapper = serializationMapper;
    }
    public Result<string> Serialize(IProtocol protocol)
    {
        var mapResult = Result.Try(() => _serializationMapper.Map(protocol));
        if (mapResult.IsFailed)
            return Result.Fail(mapResult.Errors);
        
        return Result.Try(() => JsonSerializer.Serialize(mapResult.Value, ProtocolSerializerOptions.Default));
    }
}
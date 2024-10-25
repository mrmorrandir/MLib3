using System.Text.Json;
using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Json.Common;
using MLib3.Protocols.Measurements.Serialization.Mappers;

namespace MLib3.Protocols.Measurements.Serialization.Json.Deserializers;

public class ProtocolDeserializer : IProtocolDeserializer
{
    private readonly IDeserializationMapper _deserializationMapper;

    public ProtocolDeserializer(IDeserializationMapper deserializationMapper)
    {
        _deserializationMapper = deserializationMapper;
    }
    public Result<IProtocol> Deserialize(string json)
    {
        var deserializeResult = Result.Try(() => JsonSerializer.Deserialize<Protocol>(json, ProtocolSerializerOptions.Default));
        if (deserializeResult.IsFailed)
            return Result.Fail(deserializeResult.Errors);

        var mapResult = _deserializationMapper.Map(deserializeResult.Value!);
        if (mapResult.IsFailed)
            return Result.Fail(mapResult.Errors);
        return Result.Ok(mapResult.Value);
    }
}
using System.Xml.Serialization;
using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Mappers;

namespace MLib3.Protocols.Measurements.Xml.Deserializers;

internal class ProtocolDeserializer : IProtocolDeserializer
{
    private readonly IDeserializationMapper _deserializationMapper;
    private static readonly XmlSerializer _serializer;
    

    static ProtocolDeserializer()
    {
        Type[] extraTypes = [
            typeof(Serialization.Product),
            typeof(Serialization.Meta),
            typeof(Serialization.Results),

            typeof(Serialization.Section),
            typeof(Serialization.Comment),
            typeof(Serialization.Info),
            typeof(Serialization.Flag),
            typeof(Serialization.Value),
            typeof(Serialization.RawData),

            typeof(Serialization.CommentSetting),
            typeof(Serialization.InfoSetting),
            typeof(Serialization.FlagSetting),
            typeof(Serialization.ValueSetting),
            typeof(Serialization.RawDataSetting)
        ];
        _serializer = new(typeof(Serialization.Protocol), extraTypes);
    }

    public ProtocolDeserializer(IDeserializationMapper deserializationMapper)
    {
        _deserializationMapper = deserializationMapper;
    }

    public Result<IProtocol> Deserialize(string xml)
    {
        using var reader = new StringReader(xml);
        // ReSharper disable once AccessToDisposedClosure
        var deserializeResult = Result.Try(() => (Serialization.Protocol)_serializer.Deserialize(reader));
        if (deserializeResult.IsFailed)
            return Result.Fail(deserializeResult.Errors);

        var mapResult = Result.Try(() => _deserializationMapper.Map(deserializeResult.Value));
        if (mapResult.IsFailed)
            return Result.Fail(mapResult.Errors);
        return Result.Ok(mapResult.Value);
    }
}
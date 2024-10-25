using System.Xml.Serialization;
using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Mappers;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Serializers;

internal class ProtocolSerializer : IProtocolSerializer
{
    private static readonly XmlSerializer _serializer;
    private readonly ISerializationMapper _serializationMapper;

    static ProtocolSerializer()
    {
        Type[] extraTypes =
        [
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
        _serializer = new XmlSerializer(typeof(Serialization.Protocol), extraTypes);
    }

    public ProtocolSerializer(ISerializationMapper serializationMapper)
    {
        _serializationMapper = serializationMapper;
    }

    public Result<string> Serialize(IProtocol protocol)
    {
        using var writer = new StringWriter();
        var mapResult = Result.Try(() => _serializationMapper.Map(protocol));
        if (mapResult.IsFailed)
            return Result.Fail(mapResult.Errors);

        // ReSharper disable once AccessToDisposedClosure
        var serializeResult = Result.Try(() => _serializer.Serialize(writer, mapResult.Value));
        if (serializeResult.IsFailed)
            return Result.Fail(serializeResult.Errors);

        var xml = writer.ToString();
        return Result.Ok(xml);
    }
}
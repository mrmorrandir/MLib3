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
            typeof(Product),
            typeof(Meta),
            typeof(Results),

            typeof(Section),
            typeof(Comment),
            typeof(Info),
            typeof(Flag),
            typeof(Value),
            typeof(RawData),

            typeof(CommentSetting),
            typeof(InfoSetting),
            typeof(FlagSetting),
            typeof(ValueSetting),
            typeof(RawDataSetting)
        ];
        _serializer = new XmlSerializer(typeof(Protocol), extraTypes);
    }

    public ProtocolSerializer(ISerializationMapper serializationMapper)
    {
        _serializationMapper = serializationMapper;
    }

    public Result<string> Serialize(IProtocol protocol)
    {
        using var writer = new StringWriter();
        var mapResult = _serializationMapper.Map(protocol);
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
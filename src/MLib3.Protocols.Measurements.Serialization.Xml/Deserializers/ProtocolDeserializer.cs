using System.Xml.Serialization;
using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Mappers;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Deserializers;

internal class ProtocolDeserializer : IProtocolDeserializer
{
    private readonly IDeserializationMapper _deserializationMapper;
    private static readonly XmlSerializer _serializer;
    

    static ProtocolDeserializer()
    {
        Type[] extraTypes = [
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
        _serializer = new(typeof(Protocol), extraTypes);
    }

    public ProtocolDeserializer(IDeserializationMapper deserializationMapper)
    {
        _deserializationMapper = deserializationMapper;
    }

    public Result<IProtocol> Deserialize(string xml)
    {
        using var reader = new StringReader(xml);
        // ReSharper disable once AccessToDisposedClosure
        var deserializeResult = Result.Try(() => (Protocol)_serializer.Deserialize(reader));
        if (deserializeResult.IsFailed)
            return Result.Fail(deserializeResult.Errors);

        var mapResult = _deserializationMapper.Map(deserializeResult.Value);
        if (mapResult.IsFailed)
            return Result.Fail(mapResult.Errors);
        return Result.Ok(mapResult.Value);
    }
}
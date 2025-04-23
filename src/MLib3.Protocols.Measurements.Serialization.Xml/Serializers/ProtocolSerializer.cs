using System.Xml.Serialization;
using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Serializers;

internal class ProtocolSerializer : IProtocolSerializer
{
    private static readonly XmlSerializer _serializer;

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

    public ProtocolSerializer()
    {
    }

    public Result<string> Serialize(Protocol protocol)
    {
        using var writer = new StringWriter();
        // ReSharper disable once AccessToDisposedClosure
        var serializeResult = Result.Try(() => _serializer.Serialize(writer, protocol));
        if (serializeResult.IsFailed)
            return Result.Fail(serializeResult.Errors);

        var xml = writer.ToString();
        return Result.Ok(xml);
    }
}
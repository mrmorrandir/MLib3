using System.Xml.Serialization;
using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Deserializers;

internal class ProtocolDeserializer : IProtocolDeserializer
{
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

    public ProtocolDeserializer()
    {
    }

    public Result<Protocol> Deserialize(string xml)
    {
        using var reader = new StringReader(xml);
        // ReSharper disable once AccessToDisposedClosure
        var deserializeResult = Result.Try(() => (Protocol)_serializer.Deserialize(reader));
        if (deserializeResult.IsFailed)
            return Result.Fail(deserializeResult.Errors);
        
        return Result.Ok(deserializeResult.Value);
    }
}
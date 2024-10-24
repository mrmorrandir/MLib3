using System.Xml.Serialization;
using FluentResults;

namespace MLib3.Protocols.Measurements.Xml;

public class ProtocolDeserializer : IProtocolDeserializer
{
    private readonly XmlSerializer _serializer = new(typeof(Protocol), [
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
    ]);
    
    public Result<IProtocol> Deserialize(string data)
    {
        using var reader = new StringReader(data);
        // ReSharper disable once AccessToDisposedClosure
        var deserializeResult = Result.Try(() => _serializer.Deserialize(reader) as Protocol);
        if (deserializeResult.IsFailed)
            return Result.Fail(deserializeResult.Errors);

        return Result.Ok(SerializationToProtocolMapper.Map(deserializeResult.Value!));
    }
}
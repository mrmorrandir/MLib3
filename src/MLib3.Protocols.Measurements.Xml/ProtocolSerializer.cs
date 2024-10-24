using System.Xml.Serialization;
using FluentResults;

namespace MLib3.Protocols.Measurements.Xml;

public class ProtocolSerializer : IProtocolSerializer
{
    private readonly ISerializationPostProcessor? _serializationPostProcessor;

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

    public ProtocolSerializer(ISerializationPostProcessor? serializationPostProcessor = null)
    {
        _serializationPostProcessor = serializationPostProcessor;
    }

    public Result<string> Serialize(IProtocol protocol)
    {
            using var writer = new StringWriter();
            var serializationObject = ProtocolToSerializationMapper.Map(protocol);
            
            // ReSharper disable once AccessToDisposedClosure
            var serializeResult = Result.Try(() => _serializer.Serialize(writer, serializationObject));
            if (serializeResult.IsFailed)
                return Result.Fail(serializeResult.Errors);

            var xml = writer.ToString();
            return _serializationPostProcessor is null
                ? Result.Ok(xml)
                : Result.Ok(_serializationPostProcessor.Process(xml));
    }
}
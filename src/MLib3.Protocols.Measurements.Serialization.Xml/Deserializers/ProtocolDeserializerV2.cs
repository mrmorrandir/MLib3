using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Xml.Converters;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Deserializers;

internal class ProtocolDeserializerV2 : IProtocolDeserializer
{
    private readonly ProtocolDeserializer _protocolDeserializer;
    private readonly XmlVersionConverterV2ToV3 _xmlVersionConverter;

    public ProtocolDeserializerV2(ProtocolDeserializer protocolDeserializer, XmlVersionConverterV2ToV3 xmlVersionConverter)
    {
        _protocolDeserializer = protocolDeserializer;
        _xmlVersionConverter = xmlVersionConverter;
    }

    public Result<IProtocol> Deserialize(string data)
    {
        var convertResult = Result.Try(() => _xmlVersionConverter.Convert(data));
        if (convertResult.IsFailed)
            return Result.Fail(convertResult.Errors);

        return _protocolDeserializer.Deserialize(convertResult.Value);
    }
}
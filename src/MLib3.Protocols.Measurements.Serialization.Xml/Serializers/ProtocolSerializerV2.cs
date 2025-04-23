using FluentResults;
using MLib3.Protocols.Measurements.Serialization.Xml.Converters;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Serializers;

internal class ProtocolSerializerV2 : IProtocolSerializer
{
    private readonly ProtocolSerializer _protocolSerializer;
    private readonly XmlVersionConverterV3ToV2 _xmlVersionConverter;

    public ProtocolSerializerV2(ProtocolSerializer protocolSerializer, XmlVersionConverterV3ToV2 xmlVersionConverter)
    {
        _protocolSerializer = protocolSerializer;
        _xmlVersionConverter = xmlVersionConverter;
    }

    public Result<string> Serialize(Protocol protocol)
    {
        var serializeResult = _protocolSerializer.Serialize(protocol);
        if (serializeResult.IsFailed)
            return Result.Fail(serializeResult.Errors);
        
        var convertResult = Result.Try(() => _xmlVersionConverter.Convert(serializeResult.Value));
        if (convertResult.IsFailed)
            return Result.Fail(convertResult.Errors);
        
        return Result.Ok(convertResult.Value);
    }
}
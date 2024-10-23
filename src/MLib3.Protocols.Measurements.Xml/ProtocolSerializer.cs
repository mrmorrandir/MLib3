using FluentResults;

namespace MLib3.Protocols.Measurements.Xml;

public class ProtocolSerializer : IProtocolSerializer
{
    private readonly IConverter<IProtocol, string> _protocolToStringConverter;

    public ProtocolSerializer(IConverter<IProtocol, string> protocolToStringConverter)
    {
        _protocolToStringConverter = protocolToStringConverter;
    }
    public Result<string> Serialize(IProtocol protocol)
    {
        return Result.Try(() => _protocolToStringConverter.Convert(protocol));
    }
}
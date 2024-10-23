namespace MLib3.Protocols.Measurements.Xml.V3;

internal class ProtocolToStringConverter : IProtocolToStringConverter
{
    private readonly IConverter<IProtocol, XDocument> _protocolToXDocumentConverter;

    public ProtocolToStringConverter(IConverter<IProtocol, XDocument> protocolToXDocumentConverter)
    {
        _protocolToXDocumentConverter = protocolToXDocumentConverter;
    }
    public string Convert(IProtocol input)
    {
        return _protocolToXDocumentConverter.Convert(input).ToString();
    }
}
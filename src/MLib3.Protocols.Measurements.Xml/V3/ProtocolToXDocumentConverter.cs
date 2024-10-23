namespace MLib3.Protocols.Measurements.Xml.V3;

internal class ProtocolToXDocumentConverter : IConverter<IProtocol, XDocument>
{
    private readonly IConverter<IProduct, XElement> _productToXElementConverter;
    private readonly IConverter<IMeta, XElement> _metaToXElementConverter;
    private readonly IConverter<IResults, XElement> _resultsToXElementConverter;

    public ProtocolToXDocumentConverter(IConverter<IProduct, XElement> productToXElementConverter, IConverter<IMeta, XElement> metaToXElementConverter, IConverter<IResults, XElement> resultsToXElementConverter)
    {
        _productToXElementConverter = productToXElementConverter;
        _metaToXElementConverter = metaToXElementConverter;
        _resultsToXElementConverter = resultsToXElementConverter;
    }
    
    public XDocument Convert(IProtocol input)
    {
        var protocol = new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement("Protocol",
                new XAttribute(nameof(IProtocol.Specification), input.Specification),
                new XAttribute(nameof(IProtocol.Version), input.Version),
                _productToXElementConverter.Convert(input.Product),
                _metaToXElementConverter.Convert(input.Meta),
                _resultsToXElementConverter.Convert(input.Results))
        );
        return protocol;
    }
}
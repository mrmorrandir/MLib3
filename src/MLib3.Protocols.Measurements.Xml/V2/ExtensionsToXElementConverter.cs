namespace MLib3.Protocols.Measurements.Xml.V2;

internal class ExtensionsToXElementConverter : IConverter<IExtensions, XElement>
{
    private readonly IConverter<IExtension, XElement> _extensionToXElementConverter;

    public ExtensionsToXElementConverter(IConverter<IExtension, XElement> extensionToXElementConverter)
    {
        _extensionToXElementConverter = extensionToXElementConverter;
    }
    
    public XElement Convert(IExtensions input)
    {
        var extensions = new XElement("Extensions");
        foreach (var extension in input)
        {
            extensions.Add(_extensionToXElementConverter.Convert(extension));
        }

        return extensions;
    }
}
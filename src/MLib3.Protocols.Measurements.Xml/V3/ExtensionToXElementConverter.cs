namespace MLib3.Protocols.Measurements.Xml.V3;

internal class ExtensionToXElementConverter : IConverter<IExtension, XElement>
{
    public XElement Convert(IExtension input)
    {
        var extension = new XElement(input.Key, new XCData(input.Value.ToString() ?? string.Empty));
        return extension;
    }
}
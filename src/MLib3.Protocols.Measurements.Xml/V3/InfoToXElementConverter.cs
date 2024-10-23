namespace MLib3.Protocols.Measurements.Xml.V3;

internal class InfoToXElementConverter : IConverter<IInfo, XElement>
{
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public InfoToXElementConverter(IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    
    public XElement Convert(IInfo input)
    {
        var info = new XElement("Info",
            new XAttribute("Name", input.Name),
            new XAttribute("Unit", input.Unit ?? string.Empty),
            new XElement("Value", input.Value));
        if (input.Description != null)
            info.Add(new XAttribute("Hint", input.Description));
        if (input.Precision != null)
            info.Add(new XAttribute(nameof(IInfo.Precision), input.Precision));
        if (input.Extensions != null)
            info.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        return info;
    }
}
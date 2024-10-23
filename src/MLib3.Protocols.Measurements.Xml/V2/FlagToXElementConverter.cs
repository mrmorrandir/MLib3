namespace MLib3.Protocols.Measurements.Xml.V2;

internal class FlagToXElementConverter : IConverter<IFlag, XElement>
{
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public FlagToXElementConverter(IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    
    public XElement Convert(IFlag input)
    {
        var flag = new XElement("MeasurementFlag",
            new XAttribute("Name", input.Name),
            new XAttribute("OK", input.OK));
        if (input.Description != null)
            flag.Add(new XAttribute("Hint", input.Description));
        if (input.Extensions != null)
            flag.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        return flag;
    }
}
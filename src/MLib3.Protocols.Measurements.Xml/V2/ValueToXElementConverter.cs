namespace MLib3.Protocols.Measurements.Xml.V2;

internal class ValueToXElementConverter : IConverter<IValue, XElement>
{
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public ValueToXElementConverter(IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    public XElement Convert(IValue input)
    {
        var value = new XElement("MeasurementValue",
            new XAttribute("Name", input.Name),
            new XAttribute("Unit", input.Unit ?? string.Empty),
            new XAttribute(nameof(IValue.OK), input.OK));
        if (input.Precision != null)
            value.Add(new XAttribute("Precision", input.Precision));
        if (input.Min != null)
            value.Add(new XElement("ValueLimitMinimum", input.Min));
        if (input.Nom != null)
            value.Add(new XElement("ValueNominal", input.Nom));
        if (input.Max != null)
            value.Add(new XElement("ValueLimitMaximum", input.Max));
        if (input.MinLimitType != null)
            value.Add(new XElement("ValueLimitMinimumType", input.MinLimitType));
        if (input.MaxLimitType != null)
            value.Add(new XElement("ValueLimitMaximumType", input.MaxLimitType));
        value.Add(new XElement("Value", input.Result));
        if (input.Extensions != null)
            value.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        
        return value;
    }
}
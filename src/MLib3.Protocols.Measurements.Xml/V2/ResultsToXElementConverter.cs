namespace MLib3.Protocols.Measurements.Xml.V2;

internal class ResultsToXElementConverter : IConverter<IResults, XElement>
{
    private readonly IConverter<ISection, XElement> _sectionToXElementConverter;
    private readonly IConverter<IComment, XElement> _commentToXElementConverter;
    private readonly IConverter<IInfo, XElement> _infoToXElementConverter;
    private readonly IConverter<IFlag, XElement> _flagToXElementConverter;
    private readonly IConverter<IValue, XElement> _valueToXElementConverter;
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public ResultsToXElementConverter(
        IConverter<ISection, XElement> sectionToXElementConverter,
        IConverter<IComment, XElement> commentToXElementConverter,
        IConverter<IInfo, XElement> infoToXElementConverter,
        IConverter<IFlag, XElement> flagToXElementConverter,
        IConverter<IValue, XElement> valueToXElementConverter,
        IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _sectionToXElementConverter = sectionToXElementConverter;
        _commentToXElementConverter = commentToXElementConverter;
        _infoToXElementConverter = infoToXElementConverter;
        _flagToXElementConverter = flagToXElementConverter;
        _valueToXElementConverter = valueToXElementConverter;
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    
    public XElement Convert(IResults input)
    {
        var results = new XElement("MeasurementData",
            new XAttribute("OK", input.OK));
        var data = new XElement("Measurements");
        foreach (var element in input.Data)
        {
            switch (element)
            {
                case IComment comment:
                    data.Add(_commentToXElementConverter.Convert(comment));
                    break;
                case IInfo info:
                    data.Add(_infoToXElementConverter.Convert(info));
                    break;
                case IFlag flag:
                    data.Add(_flagToXElementConverter.Convert(flag));
                    break;
                case IValue value:
                    data.Add(_valueToXElementConverter.Convert(value));
                    break;
                case ISection subSection:
                    data.Add(_sectionToXElementConverter.Convert(subSection));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown datatype to convert '{element.GetType()}'.");
            }
        }
        results.Add(data);
        if (input.Extensions != null)
            results.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        return results;
    }
}
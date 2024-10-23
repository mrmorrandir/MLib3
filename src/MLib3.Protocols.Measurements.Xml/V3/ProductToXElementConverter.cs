namespace MLib3.Protocols.Measurements.Xml.V3;

internal class ProductToXElementConverter : IConverter<IProduct, XElement>
{
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public ProductToXElementConverter(IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    public XElement Convert(IProduct input)
    {
        var product = new XElement("Product",
            new XElement("Serialnumber", input.Equipment));
        if (input.Material != null)
            product.Add(new XElement("Articlecode", input.Material));
        if (input.MaterialText != null)
            product.Add(new XElement("OrderKey", input.MaterialText));
        if (input.Order != null)
            product.Add(new XElement("ProductionOrder", input.Order));
        if (input.Extensions != null)
            product.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        return product;
    }
}
namespace MLib3.Protocols.Measurements.Xml.V3;

internal class CommentToXElementConverter : IConverter<IComment, XElement>
{
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public CommentToXElementConverter(IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    public XElement Convert(IComment input)
    {
        var comment = new XElement("Comment", 
            new XAttribute("Name", input.Name),
            new XAttribute("Text", input.Text));
        if (input.Description != null)
            comment.Add(new XAttribute("Hint", input.Description));
        if (input.Extensions != null)
            comment.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        return comment;
    }
}
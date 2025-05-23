namespace MLib3.Protocols.Measurements;

[XmlRoot("Extensions")]
public class Extensions : List<Extension>, IXmlSerializable
{
    public Extensions() {}
    public Extensions(params Extension[] extensions) : base(extensions) {}
    
    public XmlSchema GetSchema()
    {
        return null!;
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            reader.Read();
            return;
        }

        reader.ReadStartElement();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            var key = reader.Name;
            reader.ReadStartElement();
            var value = reader.ReadContentAsString();
            reader.ReadEndElement();
            Add(new Extension(key, value));
        }

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var extension in this)
        {
            writer.WriteStartElement(extension.Key);
            writer.WriteCData(extension.Value?.ToString() ?? string.Empty);
            writer.WriteEndElement();
        }
    }
}
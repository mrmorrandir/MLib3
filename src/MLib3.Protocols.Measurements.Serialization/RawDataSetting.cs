using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

/// <summary>
/// 
/// </summary>
public class RawDataSetting : Element, IXmlSerializable
// Seems like RawDataSetting must implement the IXmlSerializable interface to be able to serialize RawData, too.
// If not implemented, the XmlSerializer will throw an exception `InvalidOperation` with cryptic shit for `RawData`
{
    public string Format { get; set; } = string.Empty;

    public RawDataSetting()
    {
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
        reader.MoveToContent();
        Name = reader.GetAttribute("Name");
        Description = reader.GetAttribute("Description");
        Format = reader.GetAttribute("Format");
    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("Description", Description);
        writer.WriteAttributeString("Format", Format);
    }
}
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Xml;

public class RawData : RawDataSetting, IXmlSerializable
{
    public string Raw { get; set; } = string.Empty;
    /// <summary>
    /// Implementation of <see cref="IXmlSerializable"/>! Returns null!
    /// </summary>
    /// <returns>null</returns>
    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }
    /// <summary>
    /// Implementation of <see cref="IXmlSerializable"/>! Reads data from a <see cref="XmlReader"/> provided by the <see cref="XmlSerializer"/>.
    /// </summary>
    /// <param name="reader">A <see cref="XmlReader"/> that is passed to the method by the <see cref="XmlSerializer"/></param>
    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();
        Name = reader.GetAttribute("Name");
        Description = reader.GetAttribute("Description");
        Format = reader.GetAttribute("Format");
        if (!reader.IsEmptyElement)
            Raw = reader.ReadElementContentAsString();
        else
            reader.Read();
    }
    /// <summary>
    /// Implementation of <see cref="IXmlSerializable"/>! Writes data to a <see cref="XmlWriter"/> provided by the <see cref="XmlSerializer"/>.
    /// </summary>
    /// <param name="writer">A <see cref="XmlWriter"/> that is passed to the method by the <see cref="XmlSerializer"/></param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("Description", Description);
        writer.WriteAttributeString("Format", Format);
        if (Raw != null && Raw != string.Empty)
            writer.WriteCData(Raw);
    }
}
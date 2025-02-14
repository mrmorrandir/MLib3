using System.Xml;
using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class RawData : RawDataSetting, IXmlSerializable
{
    public string Raw { get; set; } = string.Empty;
    
    public RawData() { }
    
    public RawData(string name, string? description = null, string? format = null, string? raw = null) : base(name, description, format)
    {
        Raw = raw ?? string.Empty;
    }
    
    /// <summary>
    /// Implementation of <see cref="IXmlSerializable"/>! Returns null!
    /// </summary>
    /// <returns>null</returns>
    public override System.Xml.Schema.XmlSchema GetSchema()
    {
        return null!;
    }
    
    /// <summary>
    /// Implementation of <see cref="IXmlSerializable"/>! Reads data from a <see cref="XmlReader"/> provided by the <see cref="XmlSerializer"/>.
    /// </summary>
    /// <param name="reader">A <see cref="XmlReader"/> that is passed to the method by the <see cref="XmlSerializer"/></param>
    public override void ReadXml(XmlReader reader)
    {
        base.ReadXml(reader);
        if (!reader.IsEmptyElement)
        {
            Raw = reader.ReadElementContentAsString();
        }
        else
        {
            Raw = string.Empty;
            reader.Read();
        }
    }
    /// <summary>
    /// Implementation of <see cref="IXmlSerializable"/>! Writes data to a <see cref="XmlWriter"/> provided by the <see cref="XmlSerializer"/>.
    /// </summary>
    /// <param name="writer">A <see cref="XmlWriter"/> that is passed to the method by the <see cref="XmlSerializer"/></param>
    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteCData(Raw);
    }
}
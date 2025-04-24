namespace MLib3.Protocols.Measurements;

public class RawData : RawDataSetting, IXmlSerializable
{
    public string Raw { get; set; } = string.Empty;
    
    public RawData() { }
    
    public RawData(RawDataSetting rawDataSetting, string? raw = null) : this (rawDataSetting.Name, rawDataSetting.Description, rawDataSetting.Format, raw, rawDataSetting.Extensions){}
    
    public RawData(string name, string? description = null, string? format = null, string? raw = null, Extensions? extensions = null) : base(name, description, format, extensions)
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
    
    public override string ToString() => $"RawData: {Name}, Format: {Format}";
}
namespace MLib3.Protocols.Measurements;

/// <summary>
/// 
/// </summary>
public class RawDataSetting : Element, IXmlSerializable
// Seems like RawDataSetting must implement the IXmlSerializable interface to be able to serialize RawData, too.
// If not implemented, the XmlSerializer will throw an exception `InvalidOperation` with cryptic shit for `RawData`
{
    public string Format { get; set; } = string.Empty;

    public RawDataSetting() { }

    public RawDataSetting(string name, string? description = null, string? format = null, Extensions? extensions = null) : base(name, description, extensions)
    {
        Format = format ?? string.Empty;
    }

    public virtual System.Xml.Schema.XmlSchema GetSchema()
    {
        return null!;
    }

    public virtual void ReadXml(System.Xml.XmlReader reader)
    {
        reader.MoveToContent();
        Name = reader.GetAttribute("Name") ?? "RawDataSetting";
        Description = reader.GetAttribute("Description");
        Format = reader.GetAttribute("Format") ?? "text";
    }

    public virtual void WriteXml(System.Xml.XmlWriter writer)
    {
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("Description", Description ?? string.Empty);
        writer.WriteAttributeString("Format", Format);
    }
    
    public override string ToString() => $"RawDataSetting: {Name}, Format: {Format}";
}
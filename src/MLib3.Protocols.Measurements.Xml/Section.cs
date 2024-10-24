namespace MLib3.Protocols.Measurements.Xml;

public class Section : Element
{
    public bool Ok { get; set; } = false;

    public List<Element> Data { get; set; } = new();
}
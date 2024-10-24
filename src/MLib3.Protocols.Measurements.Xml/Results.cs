namespace MLib3.Protocols.Measurements.Xml;

public class Results
{
    public bool Ok { get; set; }
    public Extensions? Extensions { get; set; }
    public List<Element> Data { get; set; }
}
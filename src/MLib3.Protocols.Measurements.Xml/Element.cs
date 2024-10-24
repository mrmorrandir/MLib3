namespace MLib3.Protocols.Measurements.Xml;

public class Element
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public Extensions? Extensions { get; set; }
}
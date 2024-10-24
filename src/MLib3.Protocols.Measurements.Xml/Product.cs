namespace MLib3.Protocols.Measurements.Xml;

public class Product
{
    public string? Material { get; set; }
    public string? MaterialText { get; set; }
    public string? Order { get; set; }
    public string Equipment { get; set; } = string.Empty;

    public Extensions? Extensions { get; set; }
}
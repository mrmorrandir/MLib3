namespace MLib3.Protocols.Measurements;

public class Product
{
    public string Equipment { get; set; } = "Unknown";
    public string? Material { get; set; }
    public string? MaterialText { get; set; }
    public string? Order { get; set; }

    public Extensions? Extensions { get; set; } = null;
    
    public Product() {}

    public Product(string? equipment = null, string? material = null, string? materialText = null, string? order = null, Extensions? extensions = null)
    {
        Equipment = equipment ?? "Unknown";
        Material = material;
        MaterialText = materialText;
        Order = order;
        Extensions = extensions;
    }

    public override string ToString() => $"Product: Equipment: {Equipment}, Material: {Material}, MaterialText: {MaterialText}, Order: {Order}, Extensions: {Extensions}";
}
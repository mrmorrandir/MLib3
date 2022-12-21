namespace MLib3.Protocols.Measurements;

public class Product : IProduct
{
    public string? Material { get; set; }
    public string? MaterialText { get; set; }
    public string? Order { get; set; }
    public string Equipment { get; set; }

    public IExtensions? Extensions { get; set; }

    public Product() { }

    public Product(string equipment, string? material = null, string? materialText = null, string? order = null)
    {
        if (string.IsNullOrWhiteSpace(equipment))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(equipment));
        Equipment = equipment;
        Material = material;
        MaterialText = materialText;
        Order = order;
        Extensions = null;
    }
}
namespace MLib3.Protocols.Measurements;

public class Product : IProduct
{
    public string? Material { get; set; }
    public string? MaterialText { get; set; }
    public string? Order { get; set; }
    public string Equipment { get; set; }

    public IExtensions? Extensions { get; set; }

    public Product() { }

    public Product(string serial, string? material = null, string? orderKey = null, string? order = null)
    {
        if (string.IsNullOrWhiteSpace(serial))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(serial));
        Equipment = serial;
        Material = material;
        MaterialText = orderKey;
        Order = order;
    }
}
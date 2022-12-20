namespace MLib3.Protocols.Measurements;

public class Product : IProduct
{
    public string? Material { get; init; }
    public string? MaterialText { get; init; }
    public string? Order { get; init; }
    public string Equipment { get; init; }

    public IExtensions? Extensions { get; set; }

    public Product()
    {
        
    }
    
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
using System.Reflection;

namespace MLib3.Protocols.Measurements;

public class Protocol : IProtocol
{
    public IProduct Product { get; set; }
    public IMeta Meta { get; set; }
    public IResults Results { get; set; }

    public string Specification { get; set; }
    public string Version { get; set; }

    public Protocol()
    {
        Specification = GetType().Assembly.GetName().Name!;
        Version = GetType().Assembly.GetName().Version!.ToString();
        Product = new Product();
        Meta = new Meta();
        Results = new Results();
    }

    public Protocol(string type, string equipment)
    {
        Specification = GetType().Assembly.GetName().Name!;
        Version = GetType().Assembly.GetName().Version!.ToString();
        Product = new Product(equipment);
        Meta = new Meta(type, DateTime.Now);
        Results = new Results();
    }

    public Protocol(Product product, Meta meta, Results results)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        Meta = meta ?? throw new ArgumentNullException(nameof(meta));
        Results = results ?? throw new ArgumentNullException(nameof(results));
        Specification = Assembly.GetEntryAssembly()?.GetName().FullName ?? string.Empty;
        Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
    }
}
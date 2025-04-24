using System.Reflection;
using System.Runtime.CompilerServices;

namespace MLib3.Protocols.Measurements;

public class Protocol
{
    public Product Product { get; set; } = new();
    public Meta Meta { get; set; } = new();
    public Results Results { get; set; } = new();

    [XmlAttribute]
    public string Specification { get; set; } = Assembly.GetEntryAssembly()?.FullName ?? "Unknown";

    [XmlAttribute]
    public string Version { get; set; } = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unknown";

    public Protocol()
    {
    }

    public Protocol(Product product, Meta meta, Results results)
    {
        Product = product;
        Meta = meta;
        Results = results;
    }

    public override string ToString() => FormattableString.Invariant($"Protocol: Ok: {Results.Ok}, Equipment: {Product.Equipment}, Timestamp: {Meta.Timestamp}");
}
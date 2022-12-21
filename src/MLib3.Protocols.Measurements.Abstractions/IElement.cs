namespace MLib3.Protocols.Measurements.Abstractions;

public interface IElement : IExtendable
{
    string? Name { get; }
    string? Description { get; }
}
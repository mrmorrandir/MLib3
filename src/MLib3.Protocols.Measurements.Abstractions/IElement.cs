namespace MLib3.Protocols.Measurements;

public interface IElement : IExtendable
{
    string Name { get; }
    string? Description { get; }
}
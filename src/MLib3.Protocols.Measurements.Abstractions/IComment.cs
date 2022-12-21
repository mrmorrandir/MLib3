namespace MLib3.Protocols.Measurements.Abstractions;

public interface IComment : IElement
{
    string Text { get; set; }
}
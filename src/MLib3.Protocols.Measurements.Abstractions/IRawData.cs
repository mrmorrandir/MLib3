namespace MLib3.Protocols.Measurements.Abstractions;

public interface IRawData : IElement
{
    string Format { get; set; }
    string Raw { get; set; }
}
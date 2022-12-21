namespace MLib3.Protocols.Measurements.Abstractions;

public interface IInfoSetting : IElement
{
    double? Precision { get; }
    string? Unit { get; }
}
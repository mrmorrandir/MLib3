namespace MLib3.Protocols.Measurements;

public interface IInfoSetting : IElement
{
    double? Precision { get; }
    string Unit { get; }
}
namespace MLib3.Protocols.Measurements;

public interface IValueSetting : IElement
{
    string Unit { get; }
    double? Precision { get; }
    double? Min { get; }
    double? Nom { get; }
    double? Max { get; }
    ValueLimitType? MinLimitType { get; }
    ValueLimitType? MaxLimitType { get; }
}
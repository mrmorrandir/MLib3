namespace MLib3.Protocols.Measurements.Abstractions;

public interface IValueSetting : IElement
{
    double? Precision { get; }
    string? Unit { get; }

    double? Maximum { get; }
    double? Nominal { get; }
    double? Minimum { get; }
    ValueLimitType? MinimumLimitType { get; }
    ValueLimitType? MaximumLimitType { get; }
}
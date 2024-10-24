using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Xml;

public class ValueSetting : Element
{
    public string? Unit { get; set; }
    public double? Precision { get; set; }
    public double? Min { get; set; }
    public double? Nom { get; set; }
    public double? Max { get; set; }
    public ValueLimitType? MinLimitType { get; set; }
    public ValueLimitType? MaxLimitType { get; set; }
}
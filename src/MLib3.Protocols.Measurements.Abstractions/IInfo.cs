namespace MLib3.Protocols.Measurements.Abstractions;

public interface IInfo : IInfoSetting
{
    double Value { get; set; }
}
namespace MLib3.Protocols.Measurements;

public interface IInfo : IInfoSetting
{
    double Value { get; set; }
}
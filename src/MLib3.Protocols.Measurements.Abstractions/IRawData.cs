namespace MLib3.Protocols.Measurements;

public interface IRawData : IRawDataSetting
{
    string Raw { get; set; }
}
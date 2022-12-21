namespace MLib3.Protocols.Measurements.Abstractions;

public interface IExtension
{
    string Key { get; }
    object Value { get; set; }
}
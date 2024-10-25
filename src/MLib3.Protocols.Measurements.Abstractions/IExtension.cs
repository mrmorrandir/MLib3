namespace MLib3.Protocols.Measurements;

public interface IExtension
{
    string Key { get; }
    object? Value { get; set; }
}
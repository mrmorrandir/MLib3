namespace MLib3.Protocols.Measurements.Serialization;

public class Extension
{
    public string Key { get; set; } = string.Empty;
    public object? Value { get; set; } = null;
    
    public Extension()
    {
    }
    
    public Extension(string key, object? value)
    {
        Key = key;
        Value = value;
    }
}
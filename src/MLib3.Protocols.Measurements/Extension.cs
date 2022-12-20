namespace MLib3.Protocols.Measurements;

public class Extension : IExtension
{
    public Extension(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        Key = key;
        Value = value;
    }
    public string Key { get; }
    public object Value { get; set; }
}
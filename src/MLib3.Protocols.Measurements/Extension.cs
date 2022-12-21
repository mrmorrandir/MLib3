namespace MLib3.Protocols.Measurements;

public class Extension : IExtension
{
    public string Key { get; set; }
    public object Value { get; set; }

    public Extension(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        Key = key;
        Value = value;
    }
}
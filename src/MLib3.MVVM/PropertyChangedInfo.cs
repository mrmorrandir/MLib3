namespace MLib3.MVVM;

public class PropertyChangedInfo : IPropertyChangedInfo
{
    public DateTime Timestamp { get; }
    public string Name { get; }
    public object? Value { get; }
    public IViewModel Source { get; }

    public PropertyChangedInfo(string name, object? value, IViewModel source)
    {
        Timestamp = DateTime.Now;
        Name = name;
        Value = value;
        Source = source;
    }
}
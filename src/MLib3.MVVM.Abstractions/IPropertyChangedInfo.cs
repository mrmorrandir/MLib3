namespace MLib3.MVVM;

public interface IPropertyChangedInfo
{
    DateTime Timestamp { get; }
    string Name { get; }
    object? Value { get; }
    IViewModel Source { get; }
}
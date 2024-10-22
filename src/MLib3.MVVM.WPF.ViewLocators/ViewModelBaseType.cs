namespace MLib3.MVVM.WPF.ViewLocators;

public readonly record struct ViewModelBaseType(string Namespace, string Name)
{
    public string Namespace { get; } = Namespace;
    public string Name { get; } = Name;
}
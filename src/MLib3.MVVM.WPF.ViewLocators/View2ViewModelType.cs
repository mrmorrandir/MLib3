namespace MLib3.MVVM.WPF.ViewLocators;

public readonly record struct View2ViewModelType(ViewModelBaseType? ViewModelBaseType, ViewForViewModelType? ViewForViewModelType)
{
    public ViewModelBaseType? ViewModelBaseType { get; } = ViewModelBaseType;
    public ViewForViewModelType? ViewForViewModelType { get; } = ViewForViewModelType;
}
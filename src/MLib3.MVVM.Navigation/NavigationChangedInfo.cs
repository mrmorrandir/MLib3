namespace MLib3.MVVM.Navigation;

public class NavigationChangedInfo<T> where T : IViewModel
{
    public T? Before { get; }
    public T Now { get; }

    public NavigationChangedInfo(T? before, T now)
    {
        Before = before;
        Now = now;
    }
    
    // implicit operator to return the Now property
    public static implicit operator T(NavigationChangedInfo<T> info) => info.Now;
    
    // operator for equality comparison
    public static bool operator ==(NavigationChangedInfo<T> info, T viewModel) => info.Now.Equals(viewModel);

    public static bool operator !=(NavigationChangedInfo<T> info, T viewModel) => !(info == viewModel);
}
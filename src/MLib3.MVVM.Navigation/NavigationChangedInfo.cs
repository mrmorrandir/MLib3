namespace MLib3.MVVM.Navigation;

public class NavigationChangedInfo<T> : IEquatable<NavigationChangedInfo<T>> where T : IViewModel
{
    public T? Before { get; }
    public T Now { get; }

    public NavigationChangedInfo(T? before, T now)
    {
        Before = before;
        Now = now;
    }

    public bool Equals(NavigationChangedInfo<T>? other)
    {
        return other is not null
            && EqualityComparer<T?>.Default.Equals(Before, other.Before)
            && EqualityComparer<T>.Default.Equals(Now, other.Now);
    }

    public override bool Equals(object? obj)
    {
        return obj is NavigationChangedInfo<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Before, Now);
    }
    
    // implicit operator to return the Now property
    public static implicit operator T(NavigationChangedInfo<T> info) => info.Now;
    
    // operator for equality comparison
    public static bool operator ==(NavigationChangedInfo<T> info, T viewModel) => info.Now.Equals(viewModel);

    public static bool operator !=(NavigationChangedInfo<T> info, T viewModel) => !(info == viewModel);
}
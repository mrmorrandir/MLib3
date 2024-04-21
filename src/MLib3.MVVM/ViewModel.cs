using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MLib3.MVVM;

public class ViewModel : IViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        if (property == null) 
            throw new ArgumentNullException(nameof(property));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    protected virtual void Set<T>(ref T field, T value, ValueChangedCallback<T>? callback = null, [CallerMemberName] string? property = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        var oldValue = field;
        var newValue = value;
        field = newValue;
        callback?.Invoke(oldValue, newValue);
        OnPropertyChanged(property);
    }

    protected virtual void Set<T>(Func<T> get, Action<T> set, T value, ValueChangedCallback<T>? callback = null, [CallerMemberName] string? property = null)
    {
        if (EqualityComparer<T>.Default.Equals(get(), value)) return;
        var oldValue = get();
        var newValue = value;
        set(newValue);
        callback?.Invoke(oldValue, newValue);
        OnPropertyChanged(property);
    }
    
    protected virtual T Get<T>(Func<T> get) => get();
}


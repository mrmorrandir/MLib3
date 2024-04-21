using System.ComponentModel;

namespace MLib3.MVVM;

public interface IPropertyChangedHandler : IDisposable
{
    void Observe(INotifyPropertyChanged? source);
}
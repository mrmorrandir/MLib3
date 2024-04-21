using System.ComponentModel;

namespace MLib3.MVVM;

public abstract class SomePropertyChangedHandler<TCommand, TRootSource> : PropertyChangedHandler<TCommand, TRootSource> where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    public abstract void AddChild(IPropertyChangedHandler child);
}
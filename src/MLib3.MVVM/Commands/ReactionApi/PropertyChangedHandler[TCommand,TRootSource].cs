using System.ComponentModel;

namespace MLib3.MVVM;

public abstract class PropertyChangedHandler<TCommand, TRootSource> : IPropertyChangedHandler where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    protected ReactionRoot<TCommand, TRootSource> _root = null!;
    public ReactionRoot<TCommand, TRootSource> Root => _root;
    public abstract void Observe(INotifyPropertyChanged? source);
    public abstract void Dispose();
}
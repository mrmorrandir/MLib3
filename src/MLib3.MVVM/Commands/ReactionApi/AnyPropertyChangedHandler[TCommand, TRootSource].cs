using System.ComponentModel;

namespace MLib3.MVVM;

public sealed class AnyPropertyChangedHandler<TCommand, TRootSource> : PropertyChangedHandler<TCommand, TRootSource>, IPropertyChangedHandler where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    private INotifyPropertyChanged? _source;

    public AnyPropertyChangedHandler(ReactionRoot<TCommand, TRootSource> root)
    {
        _root = root;
        _root.Add(this);
    }
    
    public AnyPropertyChangedHandler(SomePropertyChangedHandler<TCommand, TRootSource> parent)
    {
        _root = parent.Root;
        parent.AddChild(this);
    }
    
    public override void Observe(INotifyPropertyChanged? source)
    {
        if (_source is not null)
        {
            _source.PropertyChanged -= Handle;
        }
        _source = source;
        if (_source is not null)
        {
            _source.PropertyChanged += Handle;
        }
    }

    private void Handle(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == null) return;
        Root.Notify();
    }
    
    public override void Dispose()
    {
        if (_source is null) return;
        _source.PropertyChanged -= Handle;
        _source = null;
    }
}
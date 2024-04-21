using System.ComponentModel;

namespace MLib3.MVVM;

public sealed class ReactionRoot<TCommand, TSource> : IDisposable where TCommand : ICommandBase where TSource : INotifyPropertyChanged
{
    private readonly List<PropertyChangedHandler<TCommand, TSource>> _handlers = new();
    
    public TSource Source { get; }
    public TCommand Command { get; }

    public ReactionRoot(TCommand command, TSource source)
    {
        Command = command;
        Source = source;
    }

    public void Add(PropertyChangedHandler<TCommand, TSource> handler)
    {
        _handlers.Add(handler);
    }

    public void Notify()
    {
        Command.RaiseCanExecuteChanged();
    }

    public void Dispose()
    {
        foreach (var handler in _handlers)
        {
            handler.Dispose();
        }
        _handlers.Clear();
    }
}
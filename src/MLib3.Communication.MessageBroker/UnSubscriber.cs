namespace MLib3.Communication.MessageBroker;

internal class UnSubscriber<T> : IDisposable
{
    private readonly ICollection<object> _actions;
    private readonly Action<T> _action;

    public UnSubscriber(ICollection<object> actions, Action<T> action)
    {
        _actions = actions;
        _action = action;
    }

    public void Dispose()
    {
        if (_actions.Contains(_action))
            _actions.Remove(_action);
    }
}
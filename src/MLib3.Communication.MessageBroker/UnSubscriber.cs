namespace MLib3.Communication.MessageBroker;

internal class UnSubscriber<T> : IDisposable
{
    private readonly List<object> _actions;
    private readonly Action<T> _action;

    public UnSubscriber(List<object> actions, Action<T> action)
    {
        _actions = actions;
        _action = action;
    }

    public void Dispose()
    {
        _actions.Remove(_action);
    }
}
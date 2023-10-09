namespace MLib3.Communication.MessageBroker;

public class MessageBroker : IMessageBroker
{
    private readonly Dictionary<Type, List<object>> _subscriptions = new();

    public void Publish<T>(T message) where T : IMessage
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (!_subscriptions.TryGetValue(typeof(T), out var actions))
        {
            var baseTypeSubscriptions = _subscriptions.Keys.Where(k => k.IsAssignableFrom(typeof(T))).ToList();
            if (!baseTypeSubscriptions.Any()) return;
            actions = baseTypeSubscriptions.SelectMany(k => _subscriptions[k]).ToList();
        }
        
        foreach (var action in actions.ToArray())
            ((Action<T>)action)(message);
    }

    public IDisposable Subscribe<T>(Action<T> action) where T : IMessage
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (!_subscriptions.TryGetValue(typeof(T), out var actions))
        {
            actions = new List<object>();
            _subscriptions.Add(typeof(T), actions);
        }
        actions.Add(action);
        return new UnSubscriber<T>(actions, action);
    }
}
namespace MLib3.Communication.MessageBroker;

public interface IMessageBroker
{
    public void Publish<T>(T message) where T : IMessage;
    public IDisposable Subscribe<T>(Action<T> action) where T : IMessage;
}
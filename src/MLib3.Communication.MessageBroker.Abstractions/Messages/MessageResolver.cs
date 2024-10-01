namespace MLib3.Communication.MessageBroker.Messages;

public sealed class MessageResolver : IDisposable
{
    private readonly IMessageBroker _messageBroker;
    private readonly Guid _messageId;
    private bool _disposedValue;

    public MessageResolver(IMessageBroker messageBroker, Guid messageId)
    {
        _messageBroker = messageBroker;
        _messageId = messageId;
    }

    public void Dispose()
    {
        if (_disposedValue) throw new ObjectDisposedException($"Resolver for Message with id {_messageId} is already disposed");
        _messageBroker.Publish(new ResolvedMessage(_messageId));
        _disposedValue = true;
    }
}
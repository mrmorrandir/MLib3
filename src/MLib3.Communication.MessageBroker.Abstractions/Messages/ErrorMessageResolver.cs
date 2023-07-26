namespace MLib3.Communication.MessageBroker.Messages;

public sealed class ErrorMessageResolver : IDisposable
{
    private readonly IMessageBroker _messageBroker;
    private readonly Guid _messageId;
    private bool _disposedValue;

    public ErrorMessageResolver(IMessageBroker messageBroker, Guid messageId)
    {
        _messageBroker = messageBroker;
        _messageId = messageId;
    }

    public void Dispose()
    {
        if (_disposedValue) throw new ObjectDisposedException($"ErrorResolver for ErrorMessage with id {_messageId} is already disposed");
        _messageBroker.Publish(new ErrorResolvedMessage(_messageId));
        _disposedValue = true;
    }
}
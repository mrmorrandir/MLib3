namespace MLib3.Communication.MessageBroker.Abstractions;

public interface IMessage
{
    Guid Id { get; }
    DateTime Timestamp { get; }
    string Text { get; }
}
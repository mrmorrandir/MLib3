using MLib3.Communication.MessageBroker.Abstractions;

namespace MLib3.Communication.MessageBroker.Messages;

public abstract class Message : IMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.Now;
    public string Text { get; init; } = string.Empty;
}

public class Message<T> : Message, IMessage
{
    public T? Data { get; init; }
    
    public Message(){}
    public Message(string text, T data)
    {
        Text = text;
        Data = data;
    }
}

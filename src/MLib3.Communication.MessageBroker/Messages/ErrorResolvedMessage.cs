using MLib3.Communication.MessageBroker.Abstractions;

namespace MLib3.Communication.MessageBroker.Messages;

public class ErrorResolvedMessage : Message, IMessage
{
    public Guid ErrorId { get; init; }
    
    public ErrorResolvedMessage(){}
    public ErrorResolvedMessage(Guid errorId)
    {
        Text = $"Error resolved ({errorId})";
        ErrorId = errorId;
    }
}
namespace MLib3.Communication.MessageBroker.Messages;

public class ResolvedMessage : Message, IMessage
{
    public Guid ErrorId { get; init; }
    
    public ResolvedMessage(){}
    public ResolvedMessage(Guid errorId)
    {
        Text = $"Error resolved ({errorId})";
        ErrorId = errorId;
    }
}
namespace MLib3.Communication.MessageBroker.Messages;

public class ErrorMessage : Message, IMessage
{
    private readonly Action<Guid>? _resetAction;
    public bool IsResettable => _resetAction != null;
    
    public ErrorMessage(){}
    public ErrorMessage(string text, Action<Guid>? resetAction = null)
    {
        Text = text;
        _resetAction = resetAction;
    }
    
    public void Reset()
    {
        _resetAction?.Invoke(Id);
    }
}
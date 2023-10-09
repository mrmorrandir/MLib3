namespace MLib3.Communication.MessageBroker.Messages;

public class TempInfoMessage : InfoMessage, IMessage
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);
    
    public TempInfoMessage(){}
    public TempInfoMessage(string text) : base(text) { }
    public TempInfoMessage(string text, int milliseconds) : base(text) { Timeout = TimeSpan.FromMilliseconds(milliseconds); }
    public TempInfoMessage(string text, TimeSpan timeout) : base(text)
    {
        Timeout = timeout;
    }
}
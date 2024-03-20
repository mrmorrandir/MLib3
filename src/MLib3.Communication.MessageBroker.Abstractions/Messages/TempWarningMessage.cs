namespace MLib3.Communication.MessageBroker.Messages;

public class TempWarningMessage : WarningMessage, IMessage
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);
    
    public TempWarningMessage(){}
    public TempWarningMessage(string text) : base(text) { }
    public TempWarningMessage(string text, int milliseconds) : base(text) { Timeout = TimeSpan.FromMilliseconds(milliseconds); }
    public TempWarningMessage(string text, TimeSpan timeout) : base(text)
    {
        Timeout = timeout;
    }
}
namespace MLib3.Communication.MessageBroker.Messages;

public class TempErrorMessage : ErrorMessage, IMessage
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);
    
    public TempErrorMessage(){}
    public TempErrorMessage(string text) : base(text) { }
    public TempErrorMessage(string text, int milliseconds) : base(text) { Timeout = TimeSpan.FromMilliseconds(milliseconds); }
    public TempErrorMessage(string text, TimeSpan timeout) : base(text)
    {
        Timeout = timeout;
    }
}
namespace MLib3.Communication.MessageBroker.Messages;

public class WarningMessage : Message, IMessage
{
    public WarningMessage(){}

    public WarningMessage(string text)
    {
        Text = text;
    }
}
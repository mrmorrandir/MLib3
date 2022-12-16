using MLib3.Communication.MessageBroker.Abstractions;

namespace MLib3.Communication.MessageBroker.Messages;

public class InfoMessage : Message, IMessage
{
    public InfoMessage(){}

    public InfoMessage(string text)
    {
        Text = text;
    }
}
using FluentAssertions;
using MLib3.Communication.MessageBroker.Messages;
using Xunit;

namespace MLib3.Communication.MessageBroker.UnitTests;

public class MessageBrokerUnitTests
{
    [Fact]
    public void ShouldSendAndReceiveInfoMessage_WhenUsingCorrespondingSubscribeAndPublish()
    {
        var messageBroker = new MessageBroker();
        var message = new InfoMessage();
        var messageReceived = false;

        messageBroker.Subscribe<InfoMessage>(m =>
        {
            messageReceived = true;
        });

        messageBroker.Publish(message);

        messageReceived.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotReceiveOtherMessageType_WhenNotSubscribedToIt()
    {
        var messageBroker = new MessageBroker();
        var messageReceived = false;

        messageBroker.Subscribe<ErrorMessage>(m =>
        {
            messageReceived = true;
        });

        messageBroker.Publish(new InfoMessage());

        messageReceived.Should().BeFalse();
    }
    
    
    
    [Fact]
    public void ShouldNotReceiveMessage_WhenUnsubscribed()
    {
        var messageBroker = new MessageBroker();
        var messageReceived = false;

        var subscription = messageBroker.Subscribe<InfoMessage>(m =>
        {
            messageReceived = true;
        });

        subscription.Dispose();
        messageBroker.Publish(new InfoMessage());

        messageReceived.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldReceiveMessage_WhenSubscribedAgain()
    {
        var messageBroker = new MessageBroker();
        var messageReceived = false;

        var subscription = messageBroker.Subscribe<InfoMessage>(m =>
        {
            messageReceived = true;
        });

        subscription.Dispose();
        subscription = messageBroker.Subscribe<InfoMessage>(m =>
        {
            messageReceived = true;
        });
        messageBroker.Publish(new InfoMessage());

        messageReceived.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(typeof(IMessage), typeof(MockBaseMessage))]
    [InlineData(typeof(IMessage), typeof(ErrorMessage))]
    [InlineData(typeof(IMessage), typeof(ErrorResolvedMessage))]
    [InlineData(typeof(IMessage), typeof(InfoMessage))]
    [InlineData(typeof(IMessage), typeof(TempInfoMessage))]
    [InlineData(typeof(IMessage), typeof(Message<double>))]
    [InlineData(typeof(Message), typeof(Message<double>))]
    public void ShouldReceiveMessage_WhenSubscribedToBaseType(Type baseType, Type messageType)
    {
        var messageBroker = new MessageBroker();
        var messageReceived = false;

        messageBroker.GetType().GetMethod(nameof(messageBroker.Subscribe))!
            .MakeGenericMethod(baseType)
            .Invoke(messageBroker, new object[] { new Action<IMessage>(m => messageReceived = true) });

        messageBroker.GetType().GetMethod(nameof(messageBroker.Publish))!
            .MakeGenericMethod(messageType)
            .Invoke(messageBroker, new object[] { Activator.CreateInstance(messageType)! });
        messageBroker.Publish((IMessage)Activator.CreateInstance(messageType)!);

        messageReceived.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotReceiveMessage_WhenSubscribedToDerivedTypeAndBaseTypeIsPublished()
    {
        var messageBroker = new MessageBroker();
        var messageReceived = false;

        messageBroker.Subscribe<TempInfoMessage>(m =>
        {
            messageReceived = true;
        });

        messageBroker.Publish(new InfoMessage());

        messageReceived.Should().BeFalse();
    }
    
    [Fact]
    public void Publish_ShouldNotThrowException_WhenNoSubscribers()
    {
        var messageBroker = new MessageBroker();
        var message = new InfoMessage();

        var func = () => messageBroker.Publish(message);
        func.Should().NotThrow();
    }
    
    [Fact]
    public void Publish_ShouldNotThrowException_WhenNoSubscribersForMessageType()
    {
        var messageBroker = new MessageBroker();
        var message = new InfoMessage();

        messageBroker.Subscribe<ErrorMessage>(m => { });

        var func = () => messageBroker.Publish(message);
        func.Should().NotThrow();
    }
    
    [Fact]
    public void Publish_ShouldThrowException_WhenMessageIsNull()
    {
        var messageBroker = new MessageBroker();

        var func = () => messageBroker.Publish<InfoMessage>(null);
        func.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void Subscribe_ShouldThrowException_WhenActionIsNull()
    {
        var messageBroker = new MessageBroker();

        var func = () => messageBroker.Subscribe<InfoMessage>(null);
        func.Should().Throw<ArgumentNullException>();
    }
}

public class MockBaseMessage : IMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.Now;
    public string Text { get; } = string.Empty;
}
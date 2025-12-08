using MLib3.Communication.MessageBroker.Messages;

namespace MLib3.Communication.MessageBroker;

public static class MessageBrokerExtensions
{
    /// <summary>
    /// Publishes an ErrorMessage which can be reset with the <paramref name="resetAction"/>.
    /// When the <paramref name="resetAction"/> is successfull it has to publish an <see cref="ResolvedMessage"/> via the MessageBroker.
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="errorMessage">The error text</param>
    /// <param name="resetAction">An Action to reset an error state. Must publish a <see cref="ResolvedMessage"/> on success in order to tell the message handler that the error message should disappear.</param>
    /// <returns>A Disposable to resolve the error</returns>
    public static IDisposable PublishError(this IMessageBroker messageBroker, string errorMessage, Action<Guid> resetAction)
    {
        var message = new ErrorMessage(errorMessage, resetAction);
        messageBroker.Publish(message);
        return new MessageResolver(messageBroker, message.Id);
    }

    /// <summary>
    /// Publishes an ErrorMessage that is 'auto-reset' (therefore is resettable by the handler).
    /// When calling this method you must not provide a special "resetAction".
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="errorMessage">The error text</param>
    /// <returns>A Disposable to resolve the error</returns>
    public static IDisposable PublishError(this IMessageBroker messageBroker, string errorMessage)
    {
        var message = new ErrorMessage(errorMessage, id => messageBroker.Publish(new ResolvedMessage(id)));
        messageBroker.Publish(message);
        return new MessageResolver(messageBroker, message.Id);
    }

    /// <summary>
    /// Publishes an ErrorMessage.
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="errorMessage">The error message object to publish</param>
    /// <returns>A Disposable to resolve the error</returns>
    public static IDisposable PublishError(this IMessageBroker messageBroker, ErrorMessage errorMessage)
    {
        messageBroker.Publish(errorMessage);
        return new MessageResolver(messageBroker, errorMessage.Id);
    }
    
    /// <summary>
    /// Publishes a temporary error message. The message handler must handle the wish for the timeout.
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="errorMessage">The error message to be shown by the message handler </param>
    /// <param name="milliseconds">The timeout for the message</param>
    public static void PublishTempError(this IMessageBroker messageBroker, string errorMessage, int milliseconds = 5000)
    {
        messageBroker.Publish(new TempErrorMessage(errorMessage, milliseconds));
    }
    
    /// <summary>
    /// Publishes a warning message. This message is not an error (it is less severe).
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="warning">The warning message to be shown by the message handler</param>
    /// <returns>A Disposable to resolve the warning</returns>
    public static IDisposable PublishWarning(this IMessageBroker messageBroker, string warning)
    {
        var warningMessage = new WarningMessage { Text = warning };
        messageBroker.Publish(warningMessage);
        return new MessageResolver(messageBroker, warningMessage.Id);
    }
    
    /// <summary>
    /// Publishes a warning message. This message is not an error (it is less severe) <paramref name="resetAction"/>.
    /// When the <paramref name="resetAction"/> is successfull it has to publish an <see cref="ResolvedMessage"/> via the MessageBroker.
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="warning">The warning message to be shown by the message handler</param>
    /// <param name="resetAction">An Action to reset an error state. Must publish a <see cref="ResolvedMessage"/> on success in order to tell the message handler that the warning message should disappear.</param>
    /// <returns>A Disposable to resolve the error</returns>
    public static IDisposable PublishWarning(this IMessageBroker messageBroker, string warning, Action<Guid> resetAction)
    {
        var warningMessage = new WarningMessage(warning, resetAction);
        messageBroker.Publish(warningMessage);
        return new MessageResolver(messageBroker, warningMessage.Id);
    }
    
    /// <summary>
    /// Publishes a warning message. This message is not an error (it is less severe).
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="warning">The warning message to be shown by the message handler</param>
    /// <param name="milliseconds">The timeout for the message</param>
    public static void PublishTempWarning(this IMessageBroker messageBroker, string warning, int milliseconds = 5000)
    {
        messageBroker.Publish(new TempWarningMessage(warning, milliseconds));
    }
    
    /// <summary>
    /// Publishes an info message.
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="info">The information to be shown by the message handler</param>
    /// <returns>A Disposable to resolve the info</returns>
    public static IDisposable PublishInfo(this IMessageBroker messageBroker, string info)
    {
        var infoMessage = new InfoMessage { Text = info };
        messageBroker.Publish(infoMessage);
        return new MessageResolver(messageBroker, infoMessage.Id);
    }

    /// <summary>
    /// Publishes a temporary info message. The message handler must handle the wish for the timeout.
    /// </summary>
    /// <param name="messageBroker">The <see cref="MessageBroker"/> to use</param>
    /// <param name="info">The information to be shown by the message handler</param>
    /// <param name="milliseconds">The timeout for the message</param>
    public static void PublishTempInfo(this IMessageBroker messageBroker, string info, int milliseconds = 5000)
    {
        messageBroker.Publish(new TempInfoMessage { Text = info, Timeout = TimeSpan.FromMilliseconds(milliseconds) });
    }

    
}
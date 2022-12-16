using Microsoft.Extensions.DependencyInjection;
using MLib3.Communication.MessageBroker.Abstractions;

namespace MLib3.Communication.MessageBroker;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, MessageBroker>();
        return services;
    }
}
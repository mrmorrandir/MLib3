using Microsoft.Extensions.DependencyInjection;
using MLib3.Communication.MessageBroker;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, MessageBroker>();
        return services;
    }
}
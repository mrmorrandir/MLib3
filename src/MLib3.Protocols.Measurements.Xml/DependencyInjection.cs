using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.Xml;

public static class DependencyInjection
{
    public static IServiceCollection AddXmlProtocolServices(this IServiceCollection services)
    {
        services.AddTransient<IProtocolSerializer, ProtocolSerializer>();
        services.AddTransient<IProtocolDeserializer, ProtocolDeserializer>();
        return services;
    }
}
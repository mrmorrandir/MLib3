using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.Serialization.Json.Deserializers;
using MLib3.Protocols.Measurements.Serialization.Json.Serializers;

namespace MLib3.Protocols.Measurements.Serialization.Json;

public static class DependencyInjection
{
    public static IServiceCollection AddJsonProtocolServices(this IServiceCollection services)
    {
        services.AddTransient<IProtocolSerializer, ProtocolSerializer>();
        services.AddTransient<IProtocolDeserializer, ProtocolDeserializer>();
        
        return services;
    }
}
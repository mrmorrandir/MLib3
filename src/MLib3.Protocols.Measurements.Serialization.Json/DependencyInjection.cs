using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MLib3.Protocols.Measurements.Serialization.Json.Deserializers;
using MLib3.Protocols.Measurements.Serialization.Json.Serializers;
using MLib3.Protocols.Measurements.Serialization.Mappers;

namespace MLib3.Protocols.Measurements.Serialization.Json;

public static class DependencyInjection
{
    public static IServiceCollection AddJsonProtocolServices(this IServiceCollection services)
    {
        services.TryAddTransient<ISerializationMapper, SerializationMapper>();
        services.TryAddTransient<IDeserializationMapper, DeserializationMapper>();
        
        services.AddTransient<IProtocolSerializer, ProtocolSerializer>();
        services.AddTransient<IProtocolDeserializer, ProtocolDeserializer>();
        
        return services;
    }
}
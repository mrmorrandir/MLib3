using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MLib3.Protocols.Measurements.Serialization.Mappers;
using MLib3.Protocols.Measurements.Serialization.Xml.Converters;
using MLib3.Protocols.Measurements.Serialization.Xml.Deserializers;
using MLib3.Protocols.Measurements.Serialization.Xml.Serializers;

namespace MLib3.Protocols.Measurements.Serialization.Xml;

public static class DependencyInjection
{
    public static IServiceCollection AddXmlProtocolServices(this IServiceCollection services, Action<ProtocolConfigurationBuilder> configAction)
    {
        var builder = new ProtocolConfigurationBuilder();
        configAction(builder);
        var config = builder.Build();
    
        services.TryAddTransient<ISerializationMapper, SerializationMapper>();
        services.TryAddTransient<IDeserializationMapper, DeserializationMapper>();

        services.TryAddTransient<ProtocolSerializer>();
        services.TryAddTransient<ProtocolDeserializer>();
        
        services.TryAddTransient<XmlVersionConverterV3ToV2>();
        services.TryAddTransient<XmlVersionConverterV2ToV3>();
        
        if (config.Version == "V2")
        {
            services.AddTransient<IProtocolSerializer, ProtocolSerializerV2>();
            services.AddTransient<IProtocolDeserializer, ProtocolDeserializerV2>();
        }
        else
        {
            services.AddTransient<IProtocolSerializer, ProtocolSerializer>();
            services.AddTransient<IProtocolDeserializer, ProtocolDeserializer>();
        }

        return services;
    }
}

public class ProtocolConfigurationBuilder
{
    private string _version = "V3";
    
    public ProtocolConfigurationBuilder UseV2()
    {
        _version = "V2";
        return this;
    }

    public ProtocolConfigurationBuilder UseV3()
    {
        _version = "V3";
        return this;
    }

    public ProtocolConfiguration Build()
    {
        return new ProtocolConfiguration(_version);
    }
}

public record ProtocolConfiguration(string Version)
{
    public string Version { get; } = Version;
}
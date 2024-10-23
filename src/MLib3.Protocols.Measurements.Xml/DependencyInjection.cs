using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.Xml;

public static class DependencyInjection
{
    public static IServiceCollection AddMeasurements3Xml(this IServiceCollection services, XmlVersion version = XmlVersion.V3)
    {
        //AddGenericConverters(services, version);

        if (version == XmlVersion.V2)
        {
            services.AddTransient<V2.IProtocolToStringConverter, V2.ProtocolToStringConverter>();
            services.Scan(selector => selector.FromAssembliesOf(typeof(V2.IProtocolToStringConverter))
                .AddClasses(filter => filter.InNamespaces("MLib3.Protocols.Measurements.Xml.V2"))
                .AsImplementedInterfaces());
        }
        else
        {
            services.AddTransient<V3.IProtocolToStringConverter, V3.ProtocolToStringConverter>();
            services.Scan(selector => selector.FromAssembliesOf(typeof(V3.IProtocolToStringConverter))
                .AddClasses(filter => filter.InNamespaces("MLib3.Protocols.Measurements.Xml.V3"))
                .AsImplementedInterfaces());
        }

        services.AddTransient<IProtocolFileWriter, ProtocolFileWriter>();
        return services;
    }

    private static void AddGenericConverters(IServiceCollection services, XmlVersion version)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (type.Namespace != null && !type.Namespace.Contains(version.ToString())) continue;
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.GetGenericTypeDefinition() == typeof(IConverter<,>))
                {
                    services.AddTransient(@interface, type);
                }
            }
        }
    }
}

public enum XmlVersion
{
    V3,
    V2
}
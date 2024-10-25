using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.Serialization.Json.Deserializers;
using MLib3.Protocols.Measurements.Serialization.Json.Serializers;
using MLib3.Protocols.Measurements.Serialization.Mappers;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Serialization.Json.UnitTests;

public class ProtocolDeserializerTests
{
    [Fact]
    public void Deserialize_ShouldSucceed()
    {
        var serviceProvider = Setup();
        var serializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var deserializer = serviceProvider.GetRequiredService<IProtocolDeserializer>();
        var protocol = DummyProtocolGenerator.Generate();
        var json = serializer.Serialize(protocol).Value;
        
        var result = deserializer.Deserialize(json);

        result.Should().BeSuccess();
    }
    
    private ServiceProvider Setup()
    {
        var services = new ServiceCollection();
        services.AddJsonProtocolServices();
        return services.BuildServiceProvider();
    }
}
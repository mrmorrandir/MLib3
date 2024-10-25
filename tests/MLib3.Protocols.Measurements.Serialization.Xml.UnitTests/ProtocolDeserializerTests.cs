using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.Serialization.Xml;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Serialization.Xml.UnitTests;

public class ProtocolDeserializerTests
{
    [Fact]
    public void Deserialize_ShouldSucceed()
    {
        var serviceProvider = Setup(config => config.UseV3());
        var protocolDeserializer = serviceProvider.GetRequiredService<IProtocolDeserializer>();
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        var xml = protocolSerializer.Serialize(protocol).Value;
        
        var result = protocolDeserializer.Deserialize(xml);
        
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void Deserialize_ShouldSucceed_ForV2()
    {
        var serviceProvider = Setup(config => config.UseV2());
        var protocolDeserializer = serviceProvider.GetRequiredService<IProtocolDeserializer>();
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        var xml = protocolSerializer.Serialize(protocol).Value;
        
        var result = protocolDeserializer.Deserialize(xml);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
    
    private ServiceProvider Setup(Action<ProtocolConfigurationBuilder> configAction)
    {
        var services = new ServiceCollection();
        services.AddXmlProtocolServices(configAction);
        return services.BuildServiceProvider();
    }
}
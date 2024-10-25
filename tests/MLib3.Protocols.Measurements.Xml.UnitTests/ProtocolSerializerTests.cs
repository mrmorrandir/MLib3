using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public class ProtocolSerializerTests
{
    
    [Fact]
    public void Serialize_ShouldSucceed()
    {
        var serviceProvider = Setup(config => config.UseV3());
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = protocolSerializer.Serialize(protocol);
        
        result.IsSuccess.Should().BeTrue();
        var validationResults = XmlSchemaValidatorV3.Validate(result.Value);
        validationResults.Should().BeSuccess();
    }

    [Fact]
    public void Serialize_ShouldSucceed_ForV2()
    {
        var serviceProvider = Setup(config => config.UseV2());
        var protocolSerializer = serviceProvider.GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = protocolSerializer.Serialize(protocol);
        
        result.IsSuccess.Should().BeTrue();
        var validationResults = XmlSchemaValidatorV2.Validate(result.Value);
        validationResults.Should().BeSuccess();
    }

    private ServiceProvider Setup(Action<ProtocolConfigurationBuilder> configAction)
    {
        var services = new ServiceCollection();
        services.AddXmlProtocolServices(configAction);
        return services.BuildServiceProvider();
    }
}
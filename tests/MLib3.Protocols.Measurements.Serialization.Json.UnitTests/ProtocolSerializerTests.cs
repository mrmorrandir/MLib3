using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Serialization.Json.UnitTests;

public class ProtocolSerializerTests
{
    [Fact]
    public void Serialize_ShouldSucceed()
    {
        var serializer = Setup().GetRequiredService<IProtocolSerializer>();
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = serializer.Serialize(protocol);

        result.Should().BeSuccess();
    }
    
    
    private ServiceProvider Setup()
    {
        var services = new ServiceCollection();
        services.AddJsonProtocolServices();
        return services.BuildServiceProvider();
    }
}
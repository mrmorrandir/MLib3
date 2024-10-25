using MLib3.Protocols.Measurements.Serialization.Json.Serializers;
using MLib3.Protocols.Measurements.Serialization.Mappers;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Serialization.Json.UnitTests;

public class ProtocolSerializerTests
{
    [Fact]
    public void Serialize_ShouldSucceed()
    {
        var serializer2 = new ProtocolSerializer(new SerializationMapper());
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = serializer2.Serialize(protocol);

        result.Should().BeSuccess();
    }
}
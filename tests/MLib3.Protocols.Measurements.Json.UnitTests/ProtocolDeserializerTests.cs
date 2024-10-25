using MLib3.Protocols.Measurements.Json.Deserializers;
using MLib3.Protocols.Measurements.Json.Serializers;
using MLib3.Protocols.Measurements.Serialization.Mappers;
using MLib3.Protocols.Measurements.TestCore;

namespace MLib3.Protocols.Measurements.Json.UnitTests;

public class ProtocolDeserializerTests
{
    [Fact]
    public void Deserialize_ShouldSucceed()
    {
        var serializer2 = new ProtocolSerializer(new SerializationMapper());
        var protocol = DummyProtocolGenerator.Generate();
        var json = serializer2.Serialize(protocol).Value;
        var deserializer2 = new ProtocolDeserializer(new DeserializationMapper());
        
        var result = deserializer2.Deserialize(json);

        result.Should().BeSuccess();
    }
}
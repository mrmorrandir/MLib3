namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public class ProtocolDeserializerTests
{
    [Fact]
    public void ShouldSucceed()
    {
        var protocolDeserializer = new ProtocolDeserializer();
        var protocol = DummyProtocolGenerator.Generate();
        var serializedProtocol = new ProtocolSerializer().Serialize(protocol).Value;
        
        var result = protocolDeserializer.Deserialize(serializedProtocol);
        
        result.IsSuccess.Should().BeTrue();
    }
}
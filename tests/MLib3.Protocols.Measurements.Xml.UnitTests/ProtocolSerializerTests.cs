namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public class ProtocolSerializerTests
{
    [Fact]
    public void ShouldSucceed()
    {
        var protocolSerializer = new ProtocolSerializer();
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = protocolSerializer.Serialize(protocol);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ShouldSucceed_ForV2()
    {
        var protocolSerializer = new ProtocolSerializer(new V2.SerializationPostProcessor());
        var protocol = DummyProtocolGenerator.Generate();
        
        var result = protocolSerializer.Serialize(protocol);
        
        result.IsSuccess.Should().BeTrue();
        var validationResults = XmlSchemaValidator.Validate(result.Value);
        validationResults.Should().BeSuccess();
    }
}
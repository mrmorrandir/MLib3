namespace MLib3.Protocols.Measurements.Json.UnitTests;

public class ProtocolSerializerTests
{
    [Fact]
    public void Serializer_ShouldSucceed_WhenProtocolIsValid()
    {
        var serializer = new ProtocolSerializer();
        var protocol = new Protocol();
        protocol.Results.Add(new Section(){ Name = "Section 1" });
        protocol.Results.Add(new CommentSetting() { Name = "Comment 1" });
        protocol.Results.Add(new Comment() { Name = "Comment 2" });
        protocol.Results.Add(new InfoSetting() { Name = "Info 1" });
        protocol.Results.Add(new Info() { Name = "Info 2" });
        protocol.Results.Add(new FlagSetting() { Name = "Flag 1" });
        protocol.Results.Add(new Flag() { Name = "Flag 2" });
        protocol.Results.Add(new ValueSetting() { Name = "Value 1" });
        protocol.Results.Add(new Value() { Name = "Value 2" });
        var func = () => serializer.Serialize(protocol);

        var serializeResult = func.Should().NotThrow().Subject;
        serializeResult.Should().BeSuccess();
        serializeResult.Value.Should().NotBeNullOrEmpty();
    }
}

public class ProtocolDeserializerTests
{
    [Fact]
    public void Deserialize_ShouldSucceed_WhenStringIsValid()
    {
        const string json = "{\n  \"Product\": {},\n  \"Meta\": {\n    \"DeviceId\": \"BOOK-FDCK23GB2A\",\n    \"DeviceName\": \"BOOK-FDCK23GB2A\",\n    \"Software\": \"ReSharperTestRunner\",\n    \"SoftwareVersion\": \"2.16.1.109\",\n    \"Timestamp\": \"2024-10-22T13:46:55.8959553+02:00\",\n    \"Operator\": \"Dark_\"\n  },\n  \"Results\": {\n    \"Data\": [\n      {\n        \"Type\": \"Section\",\n        \"Name\": \"Section 1\",\n        \"Description\": null,\n        \"OK\": false,\n        \"Extensions\": null,\n        \"Data\": []\n      },\n      {\n        \"Type\": \"CommentSetting\",\n        \"Extensions\": null,\n        \"Name\": \"Comment 1\",\n        \"Description\": null\n      },\n      {\n        \"Type\": \"Comment\",\n        \"Extensions\": null,\n        \"Name\": \"Comment 2\",\n        \"Description\": null,\n        \"Text\": null\n      },\n      {\n        \"Type\": \"InfoSetting\",\n        \"Extensions\": null,\n        \"Name\": \"Info 1\",\n        \"Description\": null,\n        \"Precision\": null,\n        \"Unit\": null\n      },\n      {\n        \"Type\": \"Info\",\n        \"Extensions\": null,\n        \"Name\": \"Info 2\",\n        \"Description\": null,\n        \"Precision\": null,\n        \"Unit\": null,\n        \"Value\": 0\n      },\n      {\n        \"Type\": \"FlagSetting\",\n        \"Extensions\": null,\n        \"Name\": \"Flag 1\",\n        \"Description\": null\n      },\n      {\n        \"Type\": \"Flag\",\n        \"Extensions\": null,\n        \"Name\": \"Flag 2\",\n        \"Description\": null,\n        \"OK\": false\n      },\n      {\n        \"Type\": \"ValueSetting\",\n        \"Extensions\": null,\n        \"Name\": \"Value 1\",\n        \"Description\": null,\n        \"Unit\": null,\n        \"Precision\": null,\n        \"Min\": null,\n        \"Nom\": null,\n        \"Max\": null,\n        \"MinLimitType\": null,\n        \"MaxLimitType\": null\n      },\n      {\n        \"Type\": \"Value\",\n        \"Extensions\": null,\n        \"Name\": \"Value 2\",\n        \"Description\": null,\n        \"Unit\": null,\n        \"Precision\": null,\n        \"Min\": null,\n        \"Nom\": null,\n        \"Max\": null,\n        \"MinLimitType\": null,\n        \"MaxLimitType\": null,\n        \"Result\": 0,\n        \"OK\": false\n      }\n    ],\n    \"OK\": false\n  },\n  \"Specification\": \"MLib3.Protocols.Measurements\",\n  \"Version\": \"1.0.0.0\"\n}";
        var deserializer = new ProtocolDeserializer();
        
        var func = () => deserializer.Deserialize(json);
        
        var deserializeResult = func.Should().NotThrow().Subject;
        deserializeResult.Should().BeSuccess();
        deserializeResult.Value.Should().NotBeNull();
    }
}
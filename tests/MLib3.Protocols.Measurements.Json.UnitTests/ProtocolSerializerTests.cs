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
        const string json = "{\n  \"product\": {\n    \"$type\": \"MLib3.Protocols.Measurements.Product, MLib3.Protocols.Measurements\"\n  },\n  \"meta\": {\n    \"$type\": \"MLib3.Protocols.Measurements.Meta, MLib3.Protocols.Measurements\",\n    \"deviceId\": \"ID16194\",\n    \"deviceName\": \"ID16194\",\n    \"software\": \"ReSharperTestRunner\",\n    \"softwareVersion\": \"2.16.1.109\",\n    \"timestamp\": \"2024-10-22T11:53:26.267179+02:00\",\n    \"operator\": \"naumaan\"\n  },\n  \"results\": {\n    \"$type\": \"MLib3.Protocols.Measurements.Results, MLib3.Protocols.Measurements\",\n    \"ok\": false,\n    \"data\": {\n      \"$type\": \"System.Linq.Enumerable+AppendPrependN`1[[MLib3.Protocols.Measurements.Abstractions.IElement, MLib3.Protocols.Measurements.Abstractions]], System.Linq\",\n      \"$values\": [\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.Section, MLib3.Protocols.Measurements\",\n          \"name\": \"Section 1\",\n          \"ok\": false,\n          \"data\": []\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.CommentSetting, MLib3.Protocols.Measurements\",\n          \"name\": \"Comment 1\"\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.Comment, MLib3.Protocols.Measurements\",\n          \"name\": \"Comment 2\"\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.InfoSetting, MLib3.Protocols.Measurements\",\n          \"name\": \"Info 1\"\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.Info, MLib3.Protocols.Measurements\",\n          \"name\": \"Info 2\",\n          \"value\": 0.0\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.FlagSetting, MLib3.Protocols.Measurements\",\n          \"name\": \"Flag 1\"\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.Flag, MLib3.Protocols.Measurements\",\n          \"name\": \"Flag 2\",\n          \"ok\": false\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.ValueSetting, MLib3.Protocols.Measurements\",\n          \"name\": \"Value 1\"\n        },\n        {\n          \"$type\": \"MLib3.Protocols.Measurements.Value, MLib3.Protocols.Measurements\",\n          \"name\": \"Value 2\",\n          \"result\": 0.0,\n          \"ok\": false\n        }\n      ]\n    }\n  },\n  \"specification\": \"MLib3.Protocols.Measurements\",\n  \"version\": \"1.0.0.0\"\n}";
        var deserializer = new ProtocolDeserializer();
        
        var func = () => deserializer.Deserialize(json);
        
        var deserializeResult = func.Should().NotThrow().Subject;
        deserializeResult.Should().BeSuccess();
        deserializeResult.Value.Should().NotBeNull();
    }
}
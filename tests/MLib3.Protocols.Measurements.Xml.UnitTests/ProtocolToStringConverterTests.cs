using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public class ProtocolToStringConverterTests 
{
    private readonly Protocol _protocol;

    public ProtocolToStringConverterTests()
    {
        _protocol = new Protocol(
            new Product("123"),
            new Meta("Test", DateTime.Now),
            new Results(ok:true));
        var section = new Section(new SectionSetting("MySection", "No Idea Section"), ok: true);
        var comment = new Comment(new CommentSetting("MyComment", "No Idea Comment"), "This is a comment!");
        var info = new Info(new InfoSetting("MyInfo", "InfoUnit", "No Idea Info"), 1.234);
        var flag = new Flag(new FlagSetting("MyFlag", "No Idea Flag"), true);
        var value = new Value(new ValueSetting("Current", "A", "Measurement with Fluke"), 1.234, true);

        section.Add(comment);
        section.Add(info);
        section.Add(flag);
        section.Add(value);
        _protocol.Results.Data.Add(section);
    }
    
    [Fact]
    public void ShouldValidateToV2Schema_WhenConverted()
    {
        var converter = new ServiceCollection().AddMeasurements3Xml(XmlVersion.V2).BuildServiceProvider()
            .GetRequiredService<IConverter<IProtocol, string>>();
        var protocolString = converter.Convert(_protocol);
        
        var schemas = new XmlSchemaSet();
        schemas.Add("", XmlReader.Create(new StringReader(File.ReadAllText("./Protocol_Schema.xsd"))));
        
        var validationErrors = new List<string>();
        XDocument.Parse(protocolString).Validate(schemas, (sender, args) => validationErrors.Add(args.Message));

        validationErrors.Should().BeEmpty("the converter works correctly");
    }
}

public class ProtocolSerializer2Tests
{
    private readonly Protocol _protocol;

    public ProtocolSerializer2Tests()
    {
        _protocol = new Protocol(
            new Product("123"),
            new Meta("Test", DateTime.Now),
            new Results(ok:true));
        var section = new Section(new SectionSetting("MySection", "No Idea Section"), ok: true);
        var comment = new Comment(new CommentSetting("MyComment", "No Idea Comment"), "This is a comment!");
        var info = new Info(new InfoSetting("MyInfo", "InfoUnit", "No Idea Info"), 1.234);
        var flag = new Flag(new FlagSetting("MyFlag", "No Idea Flag"), true);
        var value = new Value(new ValueSetting("Current", "A", "Measurement with Fluke"), 1.234, true);

        section.Add(comment);
        section.Add(info);
        section.Add(flag);
        section.Add(value);
        _protocol.Results.Data.Add(section);
    }
    
    [Fact]
    public void ShouldSucceed()
    {
        var protocolSerializer = new ProtocolSerializer2();
        
        var result = protocolSerializer.Serialize(_protocol);
        
        result.IsSuccess.Should().BeTrue();
    }
}
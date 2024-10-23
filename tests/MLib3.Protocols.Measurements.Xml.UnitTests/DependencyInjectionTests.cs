using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public class DependencyInjectionTests
{
    private readonly Protocol _protocol;

    public DependencyInjectionTests()
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
    public void ShouldFindServices_WhenDependencyInjectionIsUsed()
    {
        var services = new ServiceCollection();
        services.AddMeasurements3Xml();
        services.Should().HaveCountGreaterThan(0);

    }
    
    [Fact]
    public void ShouldInjectRequiredServices_WhenDependencyInjectionIsUsed(){
        var services = new ServiceCollection();
        services.AddMeasurements3Xml();
        var serviceProvider = services.BuildServiceProvider();
        var func = () => serviceProvider.GetRequiredService<IConverter<IProtocol, string>>();
        func.Should().NotThrow("all services are provided by dependency injection");
        var converter = func();
        var convertFunc = () => converter.Convert(_protocol);
        convertFunc.Should().NotThrow("simple protocol is easy to convert");
        var protocolString = convertFunc();
        protocolString.Should().NotBeNullOrWhiteSpace("there is data so there must be a string");
    }
}
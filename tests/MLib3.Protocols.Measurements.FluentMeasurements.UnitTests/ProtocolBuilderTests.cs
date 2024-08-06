using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ProtocolBuilderTests
{
    public IProtocolBuilderFactory GetProtocolBuilderFactory() => new ServiceCollection().AddFluentMeasurements().BuildServiceProvider().GetRequiredService<IProtocolBuilderFactory>();
    
    [Fact]
    public void ShouldBuildProtocol_WithAllOptions()
    {
        var protocol = GetProtocolBuilderFactory().Create()
            .Meta(meta => meta
                .Operator("Test operator")
                .Software("Test software")
                .SoftwareVersion("Test software version")
                .Timestamp(DateTime.Now)
                .Type("Test type")
                .DeviceId("Test device id")
                .DeviceName("Test device name")
                .TestRoutine("Test test routine")
                .TestRoutineVersion("Test test routine version")
                .Build())
            .Product(product => product
                .Equipment("Test equipment")
                .Material("Test material")
                .MaterialText("Test material text")
                .Order("Test order")
                .Build())
            .Results(results => results.OK())
            .Build();
        
        protocol.Meta.Operator.Should().Be("Test operator");
        protocol.Meta.Software.Should().Be("Test software");
        protocol.Meta.SoftwareVersion.Should().Be("Test software version");
        protocol.Meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        protocol.Meta.Type.Should().Be("Test type");
        protocol.Meta.DeviceId.Should().Be("Test device id");
        protocol.Meta.DeviceName.Should().Be("Test device name");
        protocol.Meta.TestRoutine.Should().Be("Test test routine");
        protocol.Meta.TestRoutineVersion.Should().Be("Test test routine version");
        
        protocol.Product.Equipment.Should().Be("Test equipment");
        protocol.Product.Material.Should().Be("Test material");
        protocol.Product.MaterialText.Should().Be("Test material text");
        protocol.Product.Order.Should().Be("Test order");
    }
    
    [Fact]
    public void ShouldBuildProtocol_WithOnlyRequiredOptions()
    {
        var protocol = GetProtocolBuilderFactory().Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Product(product => product
                .Equipment("Test equipment"))
            .Results(results => results.OK())
            .Build();
        
        protocol.Meta.Operator.Should().Be(Environment.UserName);
        protocol.Meta.Software.Should().Be(Assembly.GetEntryAssembly()?.GetName().Name);
        protocol.Meta.SoftwareVersion.Should().Be(Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
        protocol.Meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        protocol.Meta.Type.Should().Be("Test type");
        protocol.Meta.DeviceId.Should().Be(Environment.MachineName);
        protocol.Meta.DeviceName.Should().Be(Environment.MachineName);
        protocol.Meta.TestRoutine.Should().BeNull();
        protocol.Meta.TestRoutineVersion.Should().BeNull();
        
        protocol.Product.Equipment.Should().Be("Test equipment");
        protocol.Product.Material.Should().BeNull();
        protocol.Product.MaterialText.Should().BeNull();
        protocol.Product.Order.Should().BeNull();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenMetaWasNotSet()
    {
        Action action = () => GetProtocolBuilderFactory().Create()
            .Product(product => product
                .Equipment("Test equipment"))
            .Results(results => results.OK())
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Meta*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenProductWasNotSet()
    {
        Action action = () => GetProtocolBuilderFactory().Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Results(results => results.OK())
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Product*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultsWasNotSet()
    {
        Action action = () => GetProtocolBuilderFactory().Create()
            .Meta(meta => meta
                .Timestamp(DateTime.Now)
                .Type("Test type"))
            .Product(product => product
                .Equipment("Test equipment"))
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Results*");
    }
}
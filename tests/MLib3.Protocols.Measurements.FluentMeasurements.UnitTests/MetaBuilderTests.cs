using System.Reflection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class MetaBuilderTests
{
    [Fact]
    public void ShouldBuildMeta_WithAllOptions()
    {
        var meta = new MetaBuilder()
            .Operator("Test operator")
            .Software("Test software")
            .SoftwareVersion("Test software version")
            .Timestamp(DateTime.Now)
            .Type("Test type")
            .DeviceId("Test device id")
            .DeviceName("Test device name")
            .TestRoutine("Test test routine")
            .TestRoutineVersion("Test test routine version")
            .Build();
        
        meta.Operator.Should().Be("Test operator");
        meta.Software.Should().Be("Test software");
        meta.SoftwareVersion.Should().Be("Test software version");
        meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        meta.Type.Should().Be("Test type");
        meta.DeviceId.Should().Be("Test device id");
        meta.DeviceName.Should().Be("Test device name");
        meta.TestRoutine.Should().Be("Test test routine");
        meta.TestRoutineVersion.Should().Be("Test test routine version");
    }
    
    [Fact]
    public void ShouldBuildMeta_WithOnlyRequiredOptions()
    {
        var meta = new MetaBuilder()
            .Timestamp(DateTime.Now)
            .Type("Test type")
            .Build();

        meta.Operator.Should().Be(Environment.UserName);
        meta.Software.Should().Be(Assembly.GetEntryAssembly()?.GetName().Name);
        meta.SoftwareVersion.Should().Be(Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
        meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        meta.Type.Should().Be("Test type");
        meta.DeviceId.Should().Be(Environment.MachineName);
        meta.DeviceName.Should().Be(Environment.MachineName);
        meta.TestRoutine.Should().BeNull();
        meta.TestRoutineVersion.Should().BeNull();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenTypeWasNotSet()
    {
        Action action = () => new MetaBuilder()
            .Timestamp(DateTime.Now)
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Type*");
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenTimestampWasSetDefault()
    {
        Action action = () => new MetaBuilder()
            .Timestamp(default)
            .Type("Test type")
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Timestamp*");
    }
}
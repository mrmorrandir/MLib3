using System.Reflection;

namespace MLib3.Protocols.Measurements.UnitTests;

public class MetaTests
{
    [Fact]
    public void ShouldInitializeMeta_WhenDefaultConstructorIsUsed()
    {
        var meta = new Meta();

        meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        meta.Extensions.Should().BeNull();
        meta.Software.Should().Be(Assembly.GetEntryAssembly().GetName().Name);
        meta.SoftwareVersion.Should().Be(Assembly.GetEntryAssembly().GetName().Version.ToString(3));
        meta.TestRoutine.Should().BeNull();
        meta.TestRoutineVersion.Should().BeNull();
        meta.DeviceName.Should().Be(Environment.MachineName);
        meta.DeviceId.Should().Be(Environment.MachineName);
        meta.Type.Should().Be("Unknown");
        meta.Operator.Should().Be(Environment.UserName);
        meta.Extensions.Should().BeNull();
    }
}


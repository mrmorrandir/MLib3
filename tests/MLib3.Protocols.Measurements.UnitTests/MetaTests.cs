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
        meta.SoftwareVersion.Should().Be(Assembly.GetEntryAssembly().GetName().Version.ToString());
        meta.TestRoutine.Should().BeNull();
        meta.TestRoutineVersion.Should().BeNull();
        meta.DeviceName.Should().Be(Environment.MachineName);
        meta.DeviceId.Should().Be(Environment.MachineName);
        meta.Type.Should().BeNull();
        meta.Operator.Should().Be(Environment.UserName);
        meta.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeMeta_WhenConstructorWithParametersIsUsed()
    {
        var meta = new Meta("TestType", DateTime.Now, "TestDeviceId", "TestDeviceName", "TestSoftware", "TestSoftwareVersion", "TestRoutine", "TestRoutineVersion", "TestOperator");

        meta.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        meta.Extensions.Should().BeNull();
        meta.Software.Should().Be("TestSoftware");
        meta.SoftwareVersion.Should().Be("TestSoftwareVersion");
        meta.TestRoutine.Should().Be("TestRoutine");
        meta.TestRoutineVersion.Should().Be("TestRoutineVersion");
        meta.DeviceName.Should().Be("TestDeviceName");
        meta.DeviceId.Should().Be("TestDeviceId");
        meta.Type.Should().Be("TestType");
        meta.Operator.Should().Be("TestOperator");
        meta.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndTypeIsNull()
    {
        Action action = () => new Meta(null, DateTime.Now, "TestDeviceId", "TestDeviceName", "TestSoftware", "TestSoftwareVersion", "TestRoutine", "TestRoutineVersion", "TestOperator");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndDateTimeIsInTheFuture()
    {
        Action action = () => new Meta("TestType", DateTime.Now.AddSeconds(1), "TestDeviceId", "TestDeviceName", "TestSoftware", "TestSoftwareVersion", "TestRoutine", "TestRoutineVersion", "TestOperator");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndSoftwareIsGivenWithoutSoftwareVersion()
    {
        Action action = () => new Meta("TestType", DateTime.Now, "TestDeviceId", "TestDeviceName", "TestSoftware", null, "TestRoutine", "TestRoutineVersion", "TestOperator");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndSoftwareVersionIsGivenWithoutSoftware()
    {
        Action action = () => new Meta("TestType", DateTime.Now, "TestDeviceId", "TestDeviceName", null, "TestSoftwareVersion", "TestRoutine", "TestRoutineVersion", "TestOperator");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndTestRoutineIsGivenWithoutTestRoutineVersion()
    {
        Action action = () => new Meta("TestType", DateTime.Now, "TestDeviceId", "TestDeviceName", "TestSoftware", "TestSoftwareVersion", "TestRoutine", null, "TestOperator");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndTestRoutineVersionIsGivenWithoutTestRoutine()
    {
        Action action = () => new Meta("TestType", DateTime.Now, "TestDeviceId", "TestDeviceName", "TestSoftware", "TestSoftwareVersion", null, "TestRoutineVersion", "TestOperator");

        action.Should().Throw<ArgumentException>();
    }
}


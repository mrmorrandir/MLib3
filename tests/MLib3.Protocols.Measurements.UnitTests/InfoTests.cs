namespace MLib3.Protocols.Measurements.UnitTests;

public class InfoTests
{
    [Fact]
    public void ShouldInitializeInfo_WhenDefaultConstructorIsUsed()
    {
        var info = new Info();

        info.Name.Should().BeEmpty();
        info.Description.Should().BeNull();
        info.Unit.Should().BeEmpty();
        info.Precision.Should().BeNull();
        info.Value.Should().Be(0);
        info.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeInfo_WhenConstructorWithParametersIsUsed()
    {
        var infoSetting = new InfoSetting("TestName", "TestDescription", "TestUnit", 0.001);
        var info = new Info(infoSetting, 1.5);

        info.Name.Should().Be(infoSetting.Name);
        info.Description.Should().Be(infoSetting.Description);
        info.Unit.Should().Be(infoSetting.Unit);
        info.Precision.Should().Be(infoSetting.Precision);
        info.Value.Should().Be(1.5);
        info.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndInfoSettingIsNull()
    {
        Action action = () => new Info(null, 1.5);

        action.Should().Throw<ArgumentException>();
    }
}
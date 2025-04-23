namespace MLib3.Protocols.Measurements.UnitTests;

public class InfoSettingTests
{
    [Fact]
    public void ShouldInitializeInfoSetting_WhenDefaultConstructorIsUsed()
    {
        var infoSetting = new InfoSetting();

        infoSetting.Name.Should().BeEmpty();
        infoSetting.Description.Should().BeNull();
        infoSetting.Unit.Should().BeEmpty();
        infoSetting.Precision.Should().Be(0.0);
        infoSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeInfoSetting_WhenConstructorWithAllParametersIsUsed()
    {
        var infoSetting = new InfoSetting("TestName", "TestDescription", "TestUnit", 0.001);

        infoSetting.Name.Should().Be("TestName");
        infoSetting.Description.Should().Be("TestDescription");
        infoSetting.Unit.Should().Be("TestUnit");
        infoSetting.Precision.Should().Be(0.001);
        infoSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndNameIsNull()
    {
        Action action = () => new InfoSetting(null);

        action.Should().Throw<ArgumentException>();
    }
}
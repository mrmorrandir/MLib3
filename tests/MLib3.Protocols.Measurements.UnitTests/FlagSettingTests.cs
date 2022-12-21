namespace MLib3.Protocols.Measurements.UnitTests;

public class FlagSettingTests
{
    [Fact]
    public void ShouldInitializeFlagSetting_WhenDefaultConstructorIsUsed()
    {
        var flagSetting = new FlagSetting();

        flagSetting.Name.Should().BeNullOrWhiteSpace();
        flagSetting.Description.Should().BeNull();
        flagSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeFlagSetting_WhenConstructorWithParametersIsUsed()
    {
        var flagSetting = new FlagSetting("TestName", "TestDescription");

        flagSetting.Name.Should().Be("TestName");
        flagSetting.Description.Should().Be("TestDescription");
        flagSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndNameIsNull()
    {
        Action action = () => new FlagSetting(null, "TestDescription");

        action.Should().Throw<ArgumentException>();
    }
}
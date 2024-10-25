namespace MLib3.Protocols.Measurements.UnitTests;

public class FlagTests
{
    [Fact]
    public void ShouldInitializeFlag_WhenDefaultConstructorIsUsed()
    {
        var flag = new Flag();

        flag.Name.Should().BeEmpty();
        flag.Description.Should().BeNull();
        flag.Ok.Should().BeFalse();
        flag.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeFlag_WhenConstructorWithParametersIsUsed()
    {
        var flagSetting = new FlagSetting
        {
            Name = "TestName",
            Description = "TestDescription"
        };
        var flag = new Flag(flagSetting, true);

        flag.Name.Should().Be(flagSetting.Name);
        flag.Description.Should().Be(flagSetting.Description);
        flag.Ok.Should().BeTrue();
        flag.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndFlagSettingIsNull()
    {
        Action action = () => new Flag(null, true);

        action.Should().Throw<ArgumentException>();
    }
}
namespace MLib3.Protocols.Measurements.UnitTests;

public class ValueTests
{
    [Fact]
    public void ShouldInitializeValue_WhenDefaultConstructorIsUsed()
    {
        var value = new Value();

        value.Name.Should().BeNull();
        value.Description.Should().BeNull();
        value.Unit.Should().BeNull();
        value.Precision.Should().BeNull();
        value.Min.Should().BeNull();
        value.Max.Should().BeNull();
        value.Nom.Should().BeNull();
        value.MinLimitType.Should().BeNull();
        value.MaxLimitType.Should().BeNull();
        value.Result.Should().Be(0);
        value.Ok.Should().BeFalse();
        value.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeValue_WhenConstructorWithParametersIsUsed()
    {
        var valueSetting = new ValueSetting
        {
            Name = "Test",
            Description = "Test",
            Unit = "Test",
            Precision = 0.1,
            Min = 1,
            Max = 2,
            Nom = 1.5,
            MinLimitType = ValueLimitType.Value,
            MaxLimitType = ValueLimitType.Value
        };
        var value = new Value(valueSetting, 1.5, true);

        value.Name.Should().Be(valueSetting.Name);
        value.Description.Should().Be(valueSetting.Description);
        value.Unit.Should().Be(valueSetting.Unit);
        value.Precision.Should().Be(valueSetting.Precision);
        value.Min.Should().Be(valueSetting.Min);
        value.Max.Should().Be(valueSetting.Max);
        value.Nom.Should().Be(valueSetting.Nom);
        value.MinLimitType.Should().Be(valueSetting.MinLimitType);
        value.MaxLimitType.Should().Be(valueSetting.MaxLimitType);
        value.Result.Should().Be(1.5);
        value.Ok.Should().BeTrue();
        value.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndValueSettingsIsNull()
    {
        Action action = () => new Value(null, 1.5, true);

        action.Should().Throw<ArgumentException>();
    }
}
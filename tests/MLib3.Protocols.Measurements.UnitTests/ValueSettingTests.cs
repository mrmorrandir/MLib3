namespace MLib3.Protocols.Measurements.UnitTests;

public class ValueSettingTests
{
    [Fact]
    public void ShouldInitializeValueSetting_WhenDefaultConstructorIsUsed()
    {
        var valueSetting = new ValueSetting();

        valueSetting.Name.Should().BeNullOrWhiteSpace();
        valueSetting.Description.Should().BeNull();
        valueSetting.Unit.Should().BeNull();
        valueSetting.Precision.Should().BeNull();
        valueSetting.Min.Should().BeNull();
        valueSetting.Max.Should().BeNull();
        valueSetting.Nom.Should().BeNull();
        valueSetting.MinLimitType.Should().BeNull();
        valueSetting.MaxLimitType.Should().BeNull();
        valueSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeValueSetting_WhenConstructorWithParametersIsUsed()
    {
        var valueSetting = new ValueSetting("TestName", "TestDescription", "TestUnit", 0.1, 1, 1.5, 2, ValueLimitType.Value, ValueLimitType.Value);

        valueSetting.Name.Should().Be("TestName");
        valueSetting.Description.Should().Be("TestDescription");
        valueSetting.Unit.Should().Be("TestUnit");
        valueSetting.Precision.Should().Be(0.1);
        valueSetting.Min.Should().Be(1);
        valueSetting.Max.Should().Be(2);
        valueSetting.Nom.Should().Be(1.5);
        valueSetting.MinLimitType.Should().Be(ValueLimitType.Value);
        valueSetting.MaxLimitType.Should().Be(ValueLimitType.Value);
        valueSetting.Extensions.Should().BeNull();
    }
}
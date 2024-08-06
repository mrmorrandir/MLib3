namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ValueSettingBuilderTests
{
    [Fact]
    public void ShouldBuildValueSetting_WithAllOptions()
    {
        var setting = new ValueSettingBuilder()
            .Name("Test")
            .Description("Test description")
            .Unit("Test unit")
            .Precision(0.001)
            .Max(999)
            .Min(0)
            .Nom(500)
            .Build();
        
        setting.Name.Should().Be("Test");
        setting.Description.Should().Be("Test description");
        setting.Unit.Should().Be("Test unit");
        setting.Precision.Should().Be(0.001);
        setting.Max.Should().Be(999);
        setting.Min.Should().Be(0);
        setting.Nom.Should().Be(500);
    }
    
    [Fact]
    public void ShouldBuildValueSetting_WithOnlyRequiredOptions()
    {
        var setting = new ValueSettingBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Build();
        
        setting.Name.Should().Be("Test");
        setting.Description.Should().BeNull();
        setting.Unit.Should().Be("Test unit");
        setting.Precision.Should().BeNull();
        setting.Max.Should().BeNull();
        setting.Min.Should().BeNull();
        setting.Nom.Should().BeNull();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => new ValueSettingBuilder()
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNomIsOutsideLimits()
    {
        Action action = () => new ValueSettingBuilder()
            .Name("Test")
            .Min(0)
            .Max(100)
            .Nom(200)
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Nom*");
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenUnitWasNotSet()
    {
        Action action = () => new ValueSettingBuilder()
            .Name("Test")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Unit*");
    }

    [Fact]
    public void ShouldBuildValueSetting_WhenCreatedFromSetting()
    {
        IValueSetting setting = new ValueSetting("Test", description: "Test description", unit: "Test unit", precision: 0.001, minimum: 0, nominal: 500, maximum: 999);
        var value = new ValueSettingBuilder(setting)
            .Build();
        
        value.Name.Should().Be("Test");
        value.Description.Should().Be("Test description");
        value.Unit.Should().Be("Test unit");
        value.Precision.Should().Be(0.001);
        value.Max.Should().Be(999);
        value.Min.Should().Be(0);
        value.Nom.Should().Be(500);
    }
}


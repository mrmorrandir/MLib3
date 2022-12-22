namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class InfoBuilderTests
{
    [Fact]
    public void ShouldBuildInfo_WithAllOptions()
    {
        var info = InfoBuilder.Create()
            .Name("Test")
            .Description("Test description")
            .Unit("Test unit")
            .Precision(0.001)
            .Value(123.456)
            .Build();
        
        info.Name.Should().Be("Test");
        info.Description.Should().Be("Test description");
        info.Unit.Should().Be("Test unit");
        info.Precision.Should().Be(0.001);
        info.Value.Should().Be(123.456);
    }
    
    [Fact]
    public void ShouldBuildInfo_WithOnlyRequiredOptions()
    {
        var info = InfoBuilder.Create()
            .Name("Test")
            .Unit("Test unit")
            .Value(123.456)
            .Build();
        
        info.Name.Should().Be("Test");
        info.Description.Should().BeNull();
        info.Unit.Should().Be("Test unit");
        info.Precision.Should().BeNull();
        info.Value.Should().Be(123.456);
    }
    
    [Fact]
    public void ShouldBuildInfo_WhenInfoSettingIsGiven()
    {
        IInfoSetting setting = new InfoSetting("Test", "Test description", "Test unit", 0.001);
        var info = InfoBuilder.CreateFromSetting(setting)
            .Value(123.456)
            .Build();
        
        info.Name.Should().Be("Test");
        info.Description.Should().Be("Test description");
        info.Unit.Should().Be("Test unit");
        info.Precision.Should().Be(0.001);
        info.Value.Should().Be(123.456);
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => InfoBuilder.Create()
            .Unit("Test unit")
            .Value(123.456)
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenUnitWasNotSet()
    {
        Action action = () => InfoBuilder.Create()
            .Name("Test")
            .Value(123.456)
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Unit*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenValueWasNotSet()
    {
        Action action = () => InfoBuilder.Create()
            .Name("Test")
            .Unit("Test unit")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Value*");
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenCreatedFromSetting_AndSettingIsNull()
    {
        Action action = () => InfoBuilder.CreateFromSetting(null!);
        
        action.Should().Throw<ArgumentException>();
    }
}
namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class InfoBuilderTests
{
    [Fact]
    public void ShouldBuildInfo_WithAllOptions()
    {
        var info = new InfoBuilder()
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
        var info = new InfoBuilder()
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
        var info = new InfoBuilder(setting)
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
        Action action = () => new InfoBuilder()
            .Unit("Test unit")
            .Value(123.456)
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenUnitWasNotSet()
    {
        Action action = () => new InfoBuilder()
            .Name("Test")
            .Value(123.456)
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Unit*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenValueWasNotSet()
    {
        Action action = () => new InfoBuilder()
            .Name("Test")
            .Unit("Test unit")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Value*");
    }
}
namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class FlagBuilderTests
{
    [Fact]
    public void ShouldBuildFlag_WithAllOptions()
    {
        var flag = new FlagBuilder()
            .Name("Test")
            .Description("Test description")
            .OK()
            .Build();
        
        flag.Name.Should().Be("Test");
        flag.Description.Should().Be("Test description");
        flag.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildFlag_WithAllOptions_AndNOK()
    {
        var flag = new FlagBuilder()
            .Name("Test")
            .Description("Test description")
            .OK() // first OK
            .NOK() // then NOK
            .Build();
        
        flag.Name.Should().Be("Test");
        flag.Description.Should().Be("Test description");
        flag.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildFlag_WithOnlyRequiredOptions()
    {
        var flag = new FlagBuilder()
            .Name("Test")
            .OK()
            .Build();
        
        flag.Name.Should().Be("Test");
        flag.Description.Should().BeNull();
        flag.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildFlag_WhenFlagSettingIsGiven()
    {
        IFlagSetting setting = new FlagSetting("Test", description: "Test description");
        var flag = new FlagBuilder(setting)
            .OK()
            .Build();
        
        flag.Name.Should().Be("Test");
        flag.Description.Should().Be("Test description");
        flag.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => new FlagBuilder()
            .OK()
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => new FlagBuilder()
            .Name("Test")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*");
    }
}
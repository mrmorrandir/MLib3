namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class FlagBuilderTests
{
    [Fact]
    public void ShouldBuildFlag_WithAllOptions()
    {
        var flag = FlagBuilder.Create()
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
        var flag = FlagBuilder.Create()
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
        var flag = FlagBuilder.Create()
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
        var flag = FlagBuilder.CreateFromSetting(setting)
            .OK()
            .Build();
        
        flag.Name.Should().Be("Test");
        flag.Description.Should().Be("Test description");
        flag.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => FlagBuilder.Create()
            .OK()
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => FlagBuilder.Create()
            .Name("Test")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*");
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenCreatedFromSetting_AndSettingIsNull()
    {
        Action action = () => FlagBuilder.CreateFromSetting(null!);
        
        action.Should().Throw<ArgumentException>();
    }
}
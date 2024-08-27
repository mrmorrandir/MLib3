using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class SectionBuilderTests
{
    private ISectionBuilderFactory GetSectionBuilderFactory() => new ServiceCollection().AddFluentMeasurements().BuildServiceProvider().GetRequiredService<ISectionBuilderFactory>();
    
    [Fact]
    public void ShouldBuildSection_WithAllOptions()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .Description("Test description")
            .OK()
            .Build();
        
        section.Name.Should().Be("Test");
        section.Description.Should().Be("Test description");
        section.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildSection_WithAllOptions_AndNOK()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .Description("Test description")
            .OK() // first OK
            .NOK() // then NOK
            .Build();
        
        section.Name.Should().Be("Test");
        section.Description.Should().Be("Test description");
        section.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildSection_WithOnlyRequiredOptions()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .OK()
            .Build();
        
        section.Name.Should().Be("Test");
        section.Description.Should().BeNull();
        section.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildSection_WhenCreatedFromSetting()
    {
        ISectionSetting setting = new SectionSetting("Test", description: "Test description");
        var section = GetSectionBuilderFactory().Create(setting)
            .OK()
            .Build();
        
        section.Name.Should().Be("Test");
        section.Description.Should().Be("Test description");
        section.OK.Should().BeTrue();
    }

    [Fact]
    public void AddValue_ShouldAddValue_WhenValueBuilderIsUsed()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
            
        section.Data.Should().Contain(e => e.Name == "Test" && (e as IValue)!.Result == 123.4 && (e as IValue)!.OK );
    }
    
    [Fact]
    public void AddComment_ShouldAddComment_WhenCommentBuilderIsUsed()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddComment(builder => builder.Name("Test").Text("Test comment"))
            .OK()
            .Build();
            
        section.Data.Should().Contain(e => e.Name == "Test" && (e as IComment)!.Text == "Test comment");
    }
    
    [Fact]
    public void AddFlag_ShouldAddFlag_WhenFlagBuilderIsUsed()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddFlag(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        section.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddSection_ShouldAddSection_WhenSectionBuilderIsUsed()
    {
        var parentSection = GetSectionBuilderFactory().Create()
            .Name("Parent")
            .AddSection(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        parentSection.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddInfo_ShouldAddInfo_WhenInfoBuilderIsUsed()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddInfo(builder => builder.Name("Test").Description("Test description").Unit("Test unit").Value(123.4))
            .OK()
            .Build();

        section.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }

    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOnlyOKValues()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .Evaluate()
            .Build();

        section.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOnlyNOKValues()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        section.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOKAndNOKValues()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test1").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test2").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        section.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOnlyOKSubSections()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddSection(builder => builder.Name("Test1").OK())
            .AddSection(builder => builder.Name("Test2").OK())
            .Evaluate()
            .Build();

        section.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingMultiLevelSubSections()
    {
        var section = GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddSection(testSection => 
                testSection.Name("Test1")
                    .AddSection(testSubSection => 
                        testSubSection.Name("Test1.1")
                            .AddValue(testSubSectionValue => testSubSectionValue.Name("Test value").Result(1).Unit("TestUnit").Evaluate())
                            .Evaluate())
                    .Evaluate())
            .AddSection(builder => builder.Name("Test2").OK())
            .Evaluate()
            .Build();

        section.OK.Should().BeTrue();
    }
   
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => GetSectionBuilderFactory().Create()
            .OK()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => GetSectionBuilderFactory().Create()
            .Name("Test")
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*Evaluate*");
    }

    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingOK()
    {
        Action action = () => GetSectionBuilderFactory().Create()
            .OK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingNOK()
    {
        Action action = () => GetSectionBuilderFactory().Create()
            .NOK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingEvaluate()
    {
        Action action = () => GetSectionBuilderFactory().Create()
            .Evaluate()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenMultipleElementsHaveTheSameName()
    {
        Action action = () => GetSectionBuilderFactory().Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
        
        action.Should().Throw<ArgumentException>().WithMessage("*Test value*");
    }
}
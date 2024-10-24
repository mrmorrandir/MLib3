using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ResultsBuilderTests
{
    private IResultsBuilderFactory GetResultsBuilderFactory()
    {
        var services = new ServiceCollection();
        services.AddFluentMeasurements();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IResultsBuilderFactory>();
    }
    
    [Fact]
    public void ShouldBuildResults_WithAllOptions()
    {
        var results = GetResultsBuilderFactory().Create()
            .OK()
            .Build();
        
        results.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildResults_WithAllOptions_AndNOK()
    {
        var results = GetResultsBuilderFactory().Create()
            .OK() // first OK
            .NOK() // then NOK
            .Build();
        
        results.Ok.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildResults_WithOnlyRequiredOptions()
    {
        var results = GetResultsBuilderFactory().Create()
            .OK()
            .Build();
        
        results.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void AddValue_ShouldAddValue_WhenValueBuilderIsUsed()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
            
        results.Data.Should().Contain(e => e.Name == "Test" && (e as IValue)!.Result == 123.4 && (e as IValue)!.Ok );
    }
    
    [Fact]
    public void AddComment_ShouldAddComment_WhenCommentBuilderIsUsed()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddComment(builder => builder.Name("Test").Text("Test comment"))
            .OK()
            .Build();
            
        results.Data.Should().Contain(e => e.Name == "Test" && (e as IComment)!.Text == "Test comment");
    }
    
    [Fact]
    public void AddFlag_ShouldAddFlag_WhenFlagBuilderIsUsed()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddFlag(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        results.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddSection_ShouldAddResults_WhenSectionBuilderIsUsed()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddSection(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        results.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddInfo_ShouldAddInfo_WhenInfoBuilderIsUsed()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddInfo(builder => builder.Name("Test").Description("Test description").Unit("Test unit").Value(123.4))
            .OK()
            .Build();

        results.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }

    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOnlyOKValues()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .Evaluate()
            .Build();

        results.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOnlyNOKValues()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        results.Ok.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOKAndNOKValues()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddValue(builder => builder.Name("Test1").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test2").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        results.Ok.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOnlyOKSubSections()
    {
        var results = GetResultsBuilderFactory().Create()
            .AddSection(builder => builder.Name("Test1").OK())
            .AddSection(builder => builder.Name("Test2").OK())
            .Evaluate()
            .Build();

        results.Ok.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingMultiLevelSubSections()
    {
        var section = GetResultsBuilderFactory().Create()
            .AddSection(testResults => 
                testResults.Name("Test1")
                    .AddSection(testSubResults => 
                        testSubResults.Name("Test1.1")
                            .AddValue(testSubResultsValue => testSubResultsValue.Name("Test value").Result(1).Unit("TestUnit").Evaluate())
                            .Evaluate())
                    .Evaluate())
            .AddSection(builder => builder.Name("Test2").OK())
            .Evaluate()
            .Build();

        section.Ok.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => GetResultsBuilderFactory().Create()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*Evaluate*");
    }

    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingOK()
    {
        Action action = () => GetResultsBuilderFactory().Create()
            .OK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingNOK()
    {
        Action action = () => GetResultsBuilderFactory().Create()
            .NOK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingEvaluate()
    {
        Action action = () => GetResultsBuilderFactory().Create()
            .Evaluate()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenMultipleElementsHaveTheSameName()
    {
        Action action = () => GetResultsBuilderFactory().Create()
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
        
        action.Should().Throw<ArgumentException>().WithMessage("*Test value*");
    }
}
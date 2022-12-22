﻿using System.Collections.Immutable;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ResultsBuilderTests
{
    [Fact]
    public void ShouldBuildResults_WithAllOptions()
    {
        var results = ResultsBuilder.Create()
            .OK()
            .Build();
        
        results.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildResults_WithAllOptions_AndNOK()
    {
        var results = ResultsBuilder.Create()
            .OK() // first OK
            .NOK() // then NOK
            .Build();
        
        results.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildResults_WithOnlyRequiredOptions()
    {
        var results = ResultsBuilder.Create()
            .OK()
            .Build();
        
        results.OK.Should().BeTrue();
    }
    
    [Fact]
    public void AddValue_ShouldAddValue()
    {
        var value = new Value(new ValueSetting("Test", "Test value"), 123.4, true);
        var results = ResultsBuilder.Create()
            .AddValue(value)
            .OK()
            .Build();
        
        results.Data.Should().Contain(value);
    }

    [Fact]
    public void AddValue_ShouldAddValue_WhenValueBuilderIsUsed()
    {
        var results = ResultsBuilder.Create()
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
            
        results.Data.Should().Contain(e => e.Name == "Test" && (e as IValue)!.Result == 123.4 && (e as IValue)!.OK );
    }

    [Fact]
    public void AddComment_ShouldAddComment()
    {
        var comment = new Comment(new CommentSetting("Test", "Test comment"));
        var results = ResultsBuilder.Create()
            .AddComment(comment)
            .OK()
            .Build();
        
        results.Data.Should().Contain(comment);
    }
    
    [Fact]
    public void AddComment_ShouldAddComment_WhenCommentBuilderIsUsed()
    {
        var results = ResultsBuilder.Create()
            .AddComment(builder => builder.Name("Test").Text("Test comment"))
            .OK()
            .Build();
            
        results.Data.Should().Contain(e => e.Name == "Test" && (e as IComment)!.Text == "Test comment");
    }
    
    [Fact]
    public void AddFlag_ShouldAddFlag()
    {
        var flag = new Flag(new FlagSetting("Test", "Test description"), true);
        var results = ResultsBuilder.Create()
            .AddFlag(flag)
            .OK()
            .Build();

        results.Data.Should().Contain(flag);
    }
    
    [Fact]
    public void AddFlag_ShouldAddFlag_WhenFlagBuilderIsUsed()
    {
        var results = ResultsBuilder.Create()
            .AddFlag(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        results.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddSection_ShouldAddResults()
    {
        var section = new Section(new SectionSetting("Test", "Test description"), ImmutableList<IElement>.Empty, true);
        var results = ResultsBuilder.Create()
            .AddSection(section)
            .OK()
            .Build();

        results.Data.Should().Contain(section);
    }
    
    [Fact]
    public void AddSection_ShouldAddResults_WhenResultsBuilderIsUsed()
    {
        var results = ResultsBuilder.Create()
            .AddSection(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        results.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddInfo_ShouldAddInfo()
    {
        var info = new Info(new InfoSetting("Test", "Test description"));
        var results = ResultsBuilder.Create()
            .AddInfo(info)
            .OK()
            .Build();

        results.Data.Should().Contain(info);
    }
    
    [Fact]
    public void AddInfo_ShouldAddInfo_WhenInfoBuilderIsUsed()
    {
        var results = ResultsBuilder.Create()
            .AddInfo(builder => builder.Name("Test").Description("Test description").Unit("Test unit").Value(123.4))
            .OK()
            .Build();

        results.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }

    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOnlyOKValues()
    {
        var results = ResultsBuilder.Create()
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .Evaluate()
            .Build();

        results.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOnlyNOKValues()
    {
        var results = ResultsBuilder.Create()
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        results.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOKAndNOKValues()
    {
        var results = ResultsBuilder.Create()
            .AddValue(builder => builder.Name("Test1").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test2").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        results.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingOnlyOKSubSections()
    {
        var results = ResultsBuilder.Create()
            .AddSection(builder => builder.Name("Test1").OK())
            .AddSection(builder => builder.Name("Test2").OK())
            .Evaluate()
            .Build();

        results.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildResultsUsingEvaluate_WhenAddingMultiLevelSubSections()
    {
        var section = ResultsBuilder.Create()
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

        section.OK.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => ResultsBuilder.Create()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*Evaluate*");
    }

    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingOK()
    {
        Action action = () => ResultsBuilder.Create()
            .OK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingNOK()
    {
        Action action = () => ResultsBuilder.Create()
            .NOK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingEvaluate()
    {
        Action action = () => ResultsBuilder.Create()
            .Evaluate()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenMultipleElementsHaveTheSameName()
    {
        Action action = () => ResultsBuilder.Create()
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
        
        action.Should().Throw<ArgumentException>().WithMessage("*Test value*");
    }

    [Fact]
    public void ShouldBuildResults_WithAddMethodOverloadsWithNames()
    {
        var section = ResultsBuilder.Create()
            .AddComment("Test comment", c => c.Description("Test comment description").Text("Test comment"))
            .AddFlag("Test flag", f => f.Description("Test flag description").OK())
            .AddInfo("Test info", i => i.Description("Test info description").Unit("Test info unit").Value(123.4))
            .AddValue("Test value", v => v.Description("Test value description").Result(123.4).Unit("Test value unit").OK())
            .AddSection("SubTest", s =>s
                .Description("Test section description")
                .AddComment(c => c.Name("SubTest comment").Description("SubTest comment description").Text("Test comment"))
                .OK())
            .Evaluate()
            .Build();
        
        section.Data.Should().Contain(e => e.Name == "Test comment" && e.Description == "Test comment description");
        section.Data.Should().Contain(e => e.Name == "Test flag" && e.Description == "Test flag description");
        section.Data.Should().Contain(e => e.Name == "Test info" && e.Description == "Test info description");
        section.Data.Should().Contain(e => e.Name == "Test value" && e.Description == "Test value description");
        section.Data.Should().Contain(e => e.Name == "SubTest" && e.Description == "Test section description");
        
        section.OK.Should().BeTrue();
    }
}
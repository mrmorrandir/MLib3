using System.Collections.Immutable;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class SectionBuilderTests
{
    [Fact]
    public void ShouldBuildSection_WithAllOptions()
    {
        var section = SectionBuilder.Create()
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
        var section = SectionBuilder.Create()
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
        var section = SectionBuilder.Create()
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
        var section = SectionBuilder.CreateFromSetting(setting)
            .OK()
            .Build();
        
        section.Name.Should().Be("Test");
        section.Description.Should().Be("Test description");
        section.OK.Should().BeTrue();
    }
    
    [Fact]
    public void AddValue_ShouldAddValue()
    {
        var value = new Value(new ValueSetting("Test", "Test value"), 123.4, true);
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddValue(value)
            .OK()
            .Build();
        
        section.Data.Should().Contain(value);
    }

    [Fact]
    public void AddValue_ShouldAddValue_WhenValueBuilderIsUsed()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
            
        section.Data.Should().Contain(e => e.Name == "Test" && (e as IValue)!.Result == 123.4 && (e as IValue)!.OK );
    }

    [Fact]
    public void AddComment_ShouldAddComment()
    {
        var comment = new Comment(new CommentSetting("Test", "Test comment"));
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddComment(comment)
            .OK()
            .Build();
        
        section.Data.Should().Contain(comment);
    }
    
    [Fact]
    public void AddComment_ShouldAddComment_WhenCommentBuilderIsUsed()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddComment(builder => builder.Name("Test").Text("Test comment"))
            .OK()
            .Build();
            
        section.Data.Should().Contain(e => e.Name == "Test" && (e as IComment)!.Text == "Test comment");
    }
    
    [Fact]
    public void AddFlag_ShouldAddFlag()
    {
        var flag = new Flag(new FlagSetting("Test", "Test description"), true);
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddFlag(flag)
            .OK()
            .Build();

        section.Data.Should().Contain(flag);
    }
    
    [Fact]
    public void AddFlag_ShouldAddFlag_WhenFlagBuilderIsUsed()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddFlag(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        section.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddSection_ShouldAddSection()
    {
        var section = new Section(new SectionSetting("Test", "Test description"), ImmutableList<IElement>.Empty, true);
        var parentSection = SectionBuilder.Create()
            .Name("Parent")
            .AddSection(section)
            .OK()
            .Build();

        parentSection.Data.Should().Contain(section);
    }
    
    [Fact]
    public void AddSection_ShouldAddSection_WhenSectionBuilderIsUsed()
    {
        var parentSection = SectionBuilder.Create()
            .Name("Parent")
            .AddSection(builder => builder.Name("Test").Description("Test description").OK())
            .OK()
            .Build();

        parentSection.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }
    
    [Fact]
    public void AddInfo_ShouldAddInfo()
    {
        var info = new Info(new InfoSetting("Test", "Test description"));
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddInfo(info)
            .OK()
            .Build();

        section.Data.Should().Contain(info);
    }
    
    [Fact]
    public void AddInfo_ShouldAddInfo_WhenInfoBuilderIsUsed()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddInfo(builder => builder.Name("Test").Description("Test description").Unit("Test unit").Value(123.4))
            .OK()
            .Build();

        section.Data.Should().Contain(e => e.Name == "Test" && e.Description == "Test description");
    }

    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOnlyOKValues()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").OK())
            .Evaluate()
            .Build();

        section.OK.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOnlyNOKValues()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test").Result(123.4).Unit("Test unit").NOK())
            .Evaluate()
            .Build();

        section.OK.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldBuildSectionUsingEvaluate_WhenAddingOKAndNOKValues()
    {
        var section = SectionBuilder.Create()
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
        var section = SectionBuilder.Create()
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
        var section = SectionBuilder.Create()
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
        Action action = () => SectionBuilder.Create()
            .OK()
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenResultWasNotSet()
    {
        Action action = () => SectionBuilder.Create()
            .Name("Test")
            .Build();

        action.Should().Throw<InvalidOperationException>().WithMessage("*Result*OK*NOK*Evaluate*");
    }

    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingOK()
    {
        Action action = () => SectionBuilder.Create()
            .OK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingNOK()
    {
        Action action = () => SectionBuilder.Create()
            .NOK()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldNotThrowInvalidOperationException_WhenResultWasSet_ByUsingEvaluate()
    {
        Action action = () => SectionBuilder.Create()
            .Evaluate()
            .Build();
        
        action.Should().NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenCreatedFromSetting_AndSettingIsNull()
    {
        Action action = () => SectionBuilder.CreateFromSetting(null!);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenMultipleElementsHaveTheSameName()
    {
        Action action = () => SectionBuilder.Create()
            .Name("Test")
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .AddValue(builder => builder.Name("Test value").Result(123.4).Unit("Test unit").OK())
            .OK()
            .Build();
        
        action.Should().Throw<ArgumentException>().WithMessage("*Test value*");
    }

    [Fact]
    public void ShouldBuildSection_WithAddMethodOverloadsWithNames()
    {
        var section = SectionBuilder.Create()
            .Name("Test")
            .Description("Test description")
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
        
        section.Name.Should().Be("Test");
        section.Description.Should().Be("Test description");
        section.Data.Should().Contain(e => e.Name == "Test comment" && e.Description == "Test comment description");
        section.Data.Should().Contain(e => e.Name == "Test flag" && e.Description == "Test flag description");
        section.Data.Should().Contain(e => e.Name == "Test info" && e.Description == "Test info description");
        section.Data.Should().Contain(e => e.Name == "Test value" && e.Description == "Test value description");
        section.Data.Should().Contain(e => e.Name == "SubTest" && e.Description == "Test section description");
        
        section.OK.Should().BeTrue();
    }
}
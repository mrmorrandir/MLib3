namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class CommentBuilderTests
{
    [Fact]
    public void ShouldBuildComment_WithAllOptions()
    {
        var comment = new CommentBuilderFactory().Create()
            .Name("Test")
            .Description("Test description")
            .Text("Test text")
            .Build();
        
        comment.Name.Should().Be("Test");
        comment.Description.Should().Be("Test description");
        comment.Text.Should().Be("Test text");
    }
    
    [Fact]
    public void ShouldBuildComment_WithOnlyRequiredOptions()
    {
        var comment = new CommentBuilderFactory().Create()
            .Name("Test")
            .Text("Test text")
            .Build();
        
        comment.Name.Should().Be("Test");
        comment.Description.Should().BeNull();
        comment.Text.Should().Be("Test text");
    }
    
    [Fact]
    public void ShouldBuildComment_WhenCreatedFromCommentSetting()
    {
        ICommentSetting setting = new CommentSetting("Test", "Test description");
        var comment = new CommentBuilderFactory().Create(setting)
            .Text("Test text")
            .Build();
        
        comment.Name.Should().Be("Test");
        comment.Description.Should().Be("Test description");
        comment.Text.Should().Be("Test text");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => new CommentBuilderFactory().Create()
            .Text("Test text")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenTextWasNotSet()
    {
        Action action = () => new CommentBuilderFactory().Create()
            .Name("Test")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Text*");
    }
}
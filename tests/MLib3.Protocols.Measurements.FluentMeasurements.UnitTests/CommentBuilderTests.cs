namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class CommentBuilderTests
{
    [Fact]
    public void ShouldBuildComment_WithAllOptions()
    {
        var comment = CommentBuilder.Create()
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
        var comment = CommentBuilder.Create()
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
        var comment = CommentBuilder.CreateFromSetting(setting)
            .Text("Test text")
            .Build();
        
        comment.Name.Should().Be("Test");
        comment.Description.Should().Be("Test description");
        comment.Text.Should().Be("Test text");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenNameWasNotSet()
    {
        Action action = () => CommentBuilder.Create()
            .Text("Test text")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Name*");
    }
    
    [Fact]
    public void ShouldThrowInvalidOperationException_WhenTextWasNotSet()
    {
        Action action = () => CommentBuilder.Create()
            .Name("Test")
            .Build();
        
        action.Should().Throw<InvalidOperationException>().WithMessage("*Text*");
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenCreatedFromSetting_AndSettingIsNull()
    {
        Action action = () => CommentBuilder.CreateFromSetting(null!);
        
        action.Should().Throw<ArgumentException>();
    }
}
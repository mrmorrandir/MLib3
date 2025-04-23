namespace MLib3.Protocols.Measurements.UnitTests;

public class CommentTests
{
    [Fact]
    public void ShouldInitializeComment_WhenDefaultConstructorIsUsed()
    {
        var comment = new Comment();

        comment.Name.Should().BeEmpty();
        comment.Description.Should().BeNull();
        comment.Text.Should().BeEmpty();
        comment.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeComment_WhenConstructorWithParametersIsUsed()
    {
        var commentSetting = new CommentSetting("TestName", "TestDescription");
        var comment = new Comment(commentSetting, "TestText");

        comment.Name.Should().Be(commentSetting.Name);
        comment.Description.Should().Be(commentSetting.Description);
        comment.Text.Should().Be("TestText");
        comment.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndCommentSettingIsNull()
    {
        Action action = () => new Comment(null, "TestText");

        action.Should().Throw<Exception>();
    }
}
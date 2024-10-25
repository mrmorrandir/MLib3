namespace MLib3.Protocols.Measurements.UnitTests;

public class CommentSettingTests
{
    [Fact]
    public void ShouldInitializeCommentSetting_WhenDefaultConstructorIsUsed()
    {
        var commentSetting = new CommentSetting();

        commentSetting.Name.Should().BeEmpty();
        commentSetting.Description.Should().BeNull();
        commentSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldInitializeCommentSetting_WhenConstructorWithParametersIsUsed()
    {
        var commentSetting = new CommentSetting("TestName", "TestDescription");

        commentSetting.Name.Should().Be("TestName");
        commentSetting.Description.Should().Be("TestDescription");
        commentSetting.Extensions.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenConstructorWithParametersIsUsed_AndNameIsNull()
    {
        Action action = () => new CommentSetting(null, "TestDescription");

        action.Should().Throw<ArgumentException>();
    }
}
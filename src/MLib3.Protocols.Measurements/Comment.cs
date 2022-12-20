namespace MLib3.Protocols.Measurements;

public class Comment : IComment
{
    public Comment(ICommentSetting commentSetting, string text = "")
    {
        if (commentSetting == null) throw new ArgumentNullException(nameof(commentSetting));
        Name = commentSetting.Name;
        Hint = commentSetting.Hint;
        Text = text;
    }
    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; set; }
    public string Text { get; set; }
}
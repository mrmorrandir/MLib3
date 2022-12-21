﻿namespace MLib3.Protocols.Measurements;

public class Comment : IComment
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string Text { get; set; }
    public Comment() { }

    public Comment(ICommentSetting commentSetting, string text = "")
    {
        if (commentSetting == null) throw new ArgumentNullException(nameof(commentSetting));
        Name = commentSetting.Name;
        Description = commentSetting.Description;
        Text = text;
    }
}
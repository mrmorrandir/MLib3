namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class CommentBuilder : ICommentBuilder
{
    private readonly Comment _comment;
    private bool _isTextSet;

    private CommentBuilder(ICommentSetting? commentSetting = null)
    {
        _comment = commentSetting is null ? new Comment() : new Comment(commentSetting);
    }

    public static ICommentBuilder Create()
    {
        return new CommentBuilder();
    }
    
    public static ICommentBuilder CreateFromSetting(ICommentSetting commentSetting)
    {
        if (commentSetting == null) throw new ArgumentNullException(nameof(commentSetting));
        return new CommentBuilder(commentSetting);
    }
    
    public ICommentBuilder Name(string name)
    {
        _comment.Name = name;
        return this;
    }
    
    public ICommentBuilder Description(string description)
    {
        _comment.Description = description;
        return this;
    }
    
    public ICommentBuilder Text(string text)
    {
        _comment.Text = text;
        _isTextSet = true;
        return this;
    }
    
    public IComment Build()
    {
        if (string.IsNullOrWhiteSpace(_comment.Name))
            throw new InvalidOperationException($"{nameof(_comment.Name)} is not set");
        if (!_isTextSet)
            throw new InvalidOperationException($"{nameof(_comment.Text)} is not set.");
        return _comment;
    }
}
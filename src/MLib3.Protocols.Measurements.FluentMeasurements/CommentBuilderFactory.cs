namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class CommentBuilderFactory : ICommentBuilderFactory
{
    public CommentBuilderFactory()
    {
        
    }
    
    public ICommentBuilder Create()
    {
        return new CommentBuilder();
    }

    public ICommentBuilder Create(ICommentSetting commentSetting)
    {
        return new CommentBuilder(commentSetting);
    }
}
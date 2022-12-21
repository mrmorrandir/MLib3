namespace MLib3.Protocols.Measurements;

public class CommentSetting : ICommentSetting
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public CommentSetting() { }

    public CommentSetting(string name, string? description = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Extensions = null;
    }
}
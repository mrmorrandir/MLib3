namespace MLib3.Protocols.Measurements;

public class CommentSetting : ICommentSetting
{
    public CommentSetting(string name, string? hint = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        Name = name;
        Hint = hint;
    }

    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; set; }
}
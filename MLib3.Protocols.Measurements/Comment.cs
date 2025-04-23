namespace MLib3.Protocols.Measurements;

public class Comment : CommentSetting
{
    [XmlAttribute]
    public string Text { get; set; } = string.Empty;
    
    public Comment() {}
    
    public Comment(CommentSetting setting, string? text = null) : this(setting.Name, setting.Description, text, setting.Extensions)
    {
    }

    public Comment(string name, string? description = null, string? text = null, Extensions? extensions = null) : base(name, description, extensions)
    {
        Text = text ?? string.Empty;
    }
    
}
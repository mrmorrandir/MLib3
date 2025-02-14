using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Comment : CommentSetting
{
    [XmlAttribute]
    public string Text { get; set; } = string.Empty;
    
    public Comment() {}

    public Comment(string name, string? description = null, string? text = null) : base(name, description)
    {
        Text = text ?? string.Empty;
    }
}
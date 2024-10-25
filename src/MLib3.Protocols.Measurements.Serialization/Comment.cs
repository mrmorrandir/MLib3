using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Serialization;

public class Comment : CommentSetting
{
    [XmlAttribute]
    public string Text { get; set; } = string.Empty;
}
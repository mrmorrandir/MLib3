namespace MLib3.Protocols.Measurements.Serialization;

public class CommentSetting : Element
{
    public CommentSetting(){}

    public CommentSetting(string name, string? description = null) : base(name, description) {}
}
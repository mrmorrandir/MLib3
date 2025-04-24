namespace MLib3.Protocols.Measurements;

public class CommentSetting : Element
{
    public CommentSetting(){}

    public CommentSetting(string name, string? description = null, Extensions? extensions = null) : base(name, description, extensions) {}

    public override string ToString() => $"CommentSetting: {Name}";
}
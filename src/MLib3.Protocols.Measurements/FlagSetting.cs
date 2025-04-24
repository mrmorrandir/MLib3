namespace MLib3.Protocols.Measurements;

public class FlagSetting : Element
{
    public FlagSetting() {}

    public FlagSetting(string name, string? description = null, Extensions? extensions = null) : base(name, description, extensions) {}
    
    public override string ToString() => $"FlagSetting: {Name}";
}
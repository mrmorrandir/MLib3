namespace MLib3.Protocols.Measurements;

public class Flag : FlagSetting, IEvaluable
{
    [XmlAttribute]
    public bool Ok { get; set; }
    
    public Flag() {}
    
    public Flag(FlagSetting flagSetting, bool? ok = null) : this(flagSetting.Name, flagSetting.Description, ok, flagSetting.Extensions) {}
    
    public Flag(string name, string? description = null, bool? ok = false, Extensions? extensions = null) : base(name, description, extensions)
    {
        Ok = ok ?? false;
    }

    public bool Evaluate() => Ok;
}
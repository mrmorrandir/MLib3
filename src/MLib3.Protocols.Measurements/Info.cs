namespace MLib3.Protocols.Measurements;

public class Info : InfoSetting
{
    public double Value { get; set; }
    
    public Info() {}
    
    public Info(InfoSetting infoSetting, double? value = null) : this(infoSetting.Name, infoSetting.Description, infoSetting.Unit, infoSetting.Precision, value, infoSetting.Extensions) { }

    public Info(string name, string? description = null, string? unit = null, double? precision = 0.0, double? value = 0.0, Extensions? extensions = null) : base(name, description, unit, precision, extensions)
    {
        Value = value ?? 0.0;
    }
    
    public override string ToString() =>  FormattableString.Invariant($"Info: {Name}, Value: {Value}, Unit: {Unit}, Precision: {Precision}");
}
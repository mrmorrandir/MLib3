namespace MLib3.Protocols.Measurements.Serialization;

public class Info : InfoSetting
{
    public double Value { get; set; }
    
    public Info() {}

    public Info(string name, string? description = null, double? precision = 0.0, string? unit = null, double? value = 0.0) : base(name, description, precision, unit)
    {
        Value = value ?? 0.0;
    }
}
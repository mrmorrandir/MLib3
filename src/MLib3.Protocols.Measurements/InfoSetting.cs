namespace MLib3.Protocols.Measurements;

public class InfoSetting : Element
{
    private double _precision = 0.0;
    
    [XmlAttribute]
    public double Precision
    {
        get => _precision;
        set
        {
            PrecisionSpecified = value != 0.0;
            _precision = value;
        }
    }

    [XmlIgnore]
    [JsonIgnore]
    public bool PrecisionSpecified { get; set; }

    [XmlAttribute]
    public string Unit { get; set; } = string.Empty;
    
    public InfoSetting() {}

    public InfoSetting(string name, string? description = null, string? unit = null, double? precision = null, Extensions? extensions = null) : base(name, description, extensions)
    {
        Precision = precision ?? 0.0;
        Unit = unit ?? string.Empty;
    }
    
    public override string ToString() =>  FormattableString.Invariant($"InfoSetting: {Name}, Unit: {Unit}, Precision: {Precision}");
}
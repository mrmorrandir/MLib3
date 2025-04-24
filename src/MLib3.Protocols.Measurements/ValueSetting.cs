namespace MLib3.Protocols.Measurements;

public class ValueSetting : Element
{
    private double _precision = 0.0;
    
    [XmlAttribute]
    public string Unit { get; set; } = string.Empty;

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
    public double? Min { get; set; }
    public double? Nom { get; set; }
    public double? Max { get; set; }
    public ValueLimitType? MinLimitType { get; set; }
    public ValueLimitType? MaxLimitType { get; set; }
    
    public ValueSetting() {}
    
    public ValueSetting(string name, string? description = null, string? unit = null, double? precision = 0.0, double? min = null, double? nom = null, double? max = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null, Extensions? extensions = null) : base(name, description, extensions)
    {
        Unit = unit ?? string.Empty;
        Precision = precision ?? 0.0;
        Min = min;
        Nom = nom;
        Max = max;
        MinLimitType = minLimitType;
        MaxLimitType = maxLimitType;
    }
    
    public override string ToString() =>  FormattableString.Invariant($"ValueSetting: {Name}, Unit: {Unit}, Precision: {Precision}, Min: {Min}, Nom: {Nom}, Max: {Max}, MinLimitType: {MinLimitType}, MaxLimitType: {MaxLimitType}");
}
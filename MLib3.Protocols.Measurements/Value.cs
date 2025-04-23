namespace MLib3.Protocols.Measurements;

public class Value : ValueSetting, IEvaluable
{
    public double Result { get; set; }
    [XmlAttribute]
    public bool Ok { get; set; }
    
    public Value() {}
    
    public Value(ValueSetting valueSetting, double? result = 0.0, bool? ok = null) : this(valueSetting.Name, valueSetting.Description, valueSetting.Unit, valueSetting.Precision, valueSetting.Min, valueSetting.Nom, valueSetting.Max, valueSetting.MinLimitType, valueSetting.MaxLimitType, result, ok, valueSetting.Extensions) {}
    
    public Value(string name, string? description = null, string? unit = null, double? precision = 0.0, double? min = null, double? nom = null, double? max = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null, double? result = 0.0, bool? ok = false, Extensions? extensions = null) : base(name, description, unit, precision, min, nom, max, minLimitType, maxLimitType, extensions)
    {
        Result = result ?? 0.0;
        Ok = ok ?? false;
    }

    public bool Evaluate()
    {
        if (Min.HasValue && Max.HasValue)
        {
            Ok = Result >= Min && Result <= Max;
        } else if (Min.HasValue)
        {
            Ok = Result >= Min;
        } else if (Max.HasValue)
        {
            Ok = Result <= Max;
        } else
        {
            Ok = true;
        }
        return Ok;
    }
}
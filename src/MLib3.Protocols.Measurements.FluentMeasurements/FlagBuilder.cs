namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class FlagBuilder : IFlagBuilder
{
    private readonly Flag _flag;
    private bool _isResultSet;

    public FlagBuilder(IFlagSetting? flagSetting = null)
    {
        _flag = flagSetting is null ? new Flag() : new Flag(flagSetting);
    }
    
    public IFlagBuilder Name(string name)
    {
        _flag.Name = name;
        return this;
    }
    
    public IFlagBuilder Description(string description)
    {
        _flag.Description = description;
        return this;
    }

    public IFlagBuilder OK()
    {
        _flag.OK = true;
        _isResultSet = true;
        return this;
    }
    
    public IFlagBuilder NOK()
    {
        _flag.OK = false;
        _isResultSet = true;
        return this;
    }
    
    public IFlag Build()
    {
        if (string.IsNullOrWhiteSpace(_flag.Name))
            throw new InvalidOperationException($"{nameof(_flag.Name)} is not set");
        if (!_isResultSet)
            throw new InvalidOperationException($"Result is not set. Use {nameof(OK)} or {nameof(NOK)}");
        return _flag;
    }
}
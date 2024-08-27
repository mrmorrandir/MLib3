namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class FlagBuilderFactory : IFlagBuilderFactory
{
    public FlagBuilderFactory()
    {
        
    }

    public IFlagBuilder Create()
    {
        return new FlagBuilder();
    }

    public IFlagBuilder Create(IFlagSetting flagSetting)
    {
        if (flagSetting == null) throw new ArgumentNullException(nameof(flagSetting));
        return new FlagBuilder(flagSetting);
    }
}
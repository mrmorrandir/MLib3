namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IFlagBuilder
{
    IFlagBuilder Name(string name);
    IFlagBuilder Description(string description);
    IFlagBuilder OK();
    IFlagBuilder NOK();

    IFlag Build();
}
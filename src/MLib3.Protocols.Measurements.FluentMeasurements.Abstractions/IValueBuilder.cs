namespace MLib3.Protocols.Measurements.FluentMeasurements;


public interface IValueBuilder
{
    IValueBuilder Name(string name);
    IValueBuilder Description(string description);
    IValueBuilder Unit(string unit);
    IValueBuilder Precision(double precision);
    IValueBuilder Max(double max, ValueLimitType limitType = ValueLimitType.Value);
    IValueBuilder Nom(double nominal);
    IValueBuilder Min(double min, ValueLimitType limitType = ValueLimitType.Value);
    IValueBuilder Result(double value);
    IValueBuilder OK();
    IValueBuilder NOK();
    IValueBuilder Evaluate();

    IValue Build();
}
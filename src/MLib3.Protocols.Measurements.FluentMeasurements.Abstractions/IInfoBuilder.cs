namespace MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

public interface IInfoBuilder
{
    IInfoBuilder Name(string name);
    IInfoBuilder Description(string description);
    IInfoBuilder Unit(string unit);
    IInfoBuilder Precision(double precision);
    IInfoBuilder Value(double value);

    IInfo Build();
}
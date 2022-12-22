namespace MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

public interface ISectionBuilder : IResultsBuilder<ISectionBuilder, ISection>
{
    ISectionBuilder Name(string name);
    ISectionBuilder Description(string description);
}

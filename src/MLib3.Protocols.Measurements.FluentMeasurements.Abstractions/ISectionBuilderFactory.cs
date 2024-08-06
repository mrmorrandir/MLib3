namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface ISectionBuilderFactory
{
    ISectionBuilder Create();
    ISectionBuilder Create(ISectionSetting setting);
}
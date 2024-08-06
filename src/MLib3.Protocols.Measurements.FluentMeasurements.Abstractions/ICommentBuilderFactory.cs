namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface ICommentBuilderFactory
{
    ICommentBuilder Create();
    ICommentBuilder Create(ICommentSetting commentSetting);
}
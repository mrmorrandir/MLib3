namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface ICommentBuilder
{
    ICommentBuilder Name(string name);
    ICommentBuilder Description(string description);
    ICommentBuilder Text(string text);

    IComment Build();
}
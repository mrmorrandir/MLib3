namespace MLib3.Protocols.Measurements.FluentMeasurements;

public interface IResultsBuilder<out TResultsBuilderInterface, out TResultsInterface>
{
    TResultsBuilderInterface AddValue(Action<IValueBuilder> valueBuilder);
    TResultsBuilderInterface AddComment(Action<ICommentBuilder> commentBuilder);
    TResultsBuilderInterface AddFlag(Action<IFlagBuilder> flagBuilder);
    TResultsBuilderInterface AddInfo(Action<IInfoBuilder> infoBuilder);
    TResultsBuilderInterface AddSection(Action<ISectionBuilder> sectionBuilder);
    TResultsBuilderInterface OK();
    TResultsBuilderInterface NOK();
    TResultsBuilderInterface Evaluate();
    TResultsInterface Build();
}

public interface IResultsBuilder : IResultsBuilder<IResultsBuilder, IResults>{}
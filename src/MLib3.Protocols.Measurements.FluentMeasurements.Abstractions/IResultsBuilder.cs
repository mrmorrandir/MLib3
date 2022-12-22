namespace MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

public interface IResultsBuilder<out TResultsBuilderInterface, out TResultsInterface>
{
    TResultsBuilderInterface AddValue(Action<IValueBuilder> valueBuilder);
    TResultsBuilderInterface AddValue(string name, Action<IValueBuilder> valueBuilder);
    TResultsBuilderInterface AddValue(IValue value);
    TResultsBuilderInterface AddComment(Action<ICommentBuilder> commentBuilder);
    TResultsBuilderInterface AddComment(string name, Action<ICommentBuilder> commentBuilder);
    TResultsBuilderInterface AddComment(IComment comment);
    TResultsBuilderInterface AddFlag(Action<IFlagBuilder> flagBuilder);
    TResultsBuilderInterface AddFlag(string name, Action<IFlagBuilder> flagBuilder);
    TResultsBuilderInterface AddFlag(IFlag flag);
    TResultsBuilderInterface AddInfo(Action<IInfoBuilder> infoBuilder);
    TResultsBuilderInterface AddInfo(string name, Action<IInfoBuilder> infoBuilder);
    TResultsBuilderInterface AddInfo(IInfo info);
    TResultsBuilderInterface AddSection(Action<ISectionBuilder> sectionBuilder);
    TResultsBuilderInterface AddSection(string name, Action<ISectionBuilder> sectionBuilder);
    TResultsBuilderInterface AddSection(ISection section);
    TResultsBuilderInterface OK();
    TResultsBuilderInterface NOK();
    TResultsBuilderInterface Evaluate();
    TResultsInterface Build();
}

public interface IResultsBuilder : IResultsBuilder<IResultsBuilder, IResults>{}
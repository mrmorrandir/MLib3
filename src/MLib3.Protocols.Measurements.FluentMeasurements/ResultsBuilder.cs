using MLib3.Protocols.Measurements.FluentMeasurements.Abstractions;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ResultsBuilder : IResultsBuilder
{
    private readonly Results _results;
    private readonly List<Func<IElement>> _builders = new();
    private bool _isResultSet;
    private bool _evaluate;

    private ResultsBuilder()
    {
        _results = new Results();
    }
    
    public static IResultsBuilder Create()
    {
        return new ResultsBuilder();
    }

    public IResultsBuilder AddValue(Action<IValueBuilder> valueBuilder)
    {
        var builder = ValueBuilder.Create();
        valueBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddValue(string name, Action<IValueBuilder> valueBuilder)
    {
        var builder = ValueBuilder.Create().Name(name);
        valueBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }

    public IResultsBuilder AddValue(IValue value)
    {
        _builders.Add(() => value);
        return this;
    }
    
    public IResultsBuilder AddComment(Action<ICommentBuilder> commentBuilder)
    {
        var builder = CommentBuilder.Create();
        commentBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    public IResultsBuilder AddComment(string name, Action<ICommentBuilder> commentBuilder)
    {
        var builder = CommentBuilder.Create().Name(name);
        commentBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddComment(IComment comment)
    {
        _builders.Add(() => comment);
        return this;
    }
    
    public IResultsBuilder AddFlag(Action<IFlagBuilder> flagBuilder)
    {
        var builder = FlagBuilder.Create();
        flagBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddFlag(string name, Action<IFlagBuilder> flagBuilder)
    {
        var builder = FlagBuilder.Create().Name(name);
        flagBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddFlag(IFlag flag)
    {
        _builders.Add(() => flag);
        return this;
    }
    
    public IResultsBuilder AddInfo(Action<IInfoBuilder> infoBuilder)
    {
        var builder = InfoBuilder.Create();
        infoBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddInfo(string name, Action<IInfoBuilder> infoBuilder)
    {
        var builder = InfoBuilder.Create().Name(name);
        infoBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddInfo(IInfo info)
    {
        _builders.Add(() => info);
        return this;
    }
    public IResultsBuilder AddSection(Action<ISectionBuilder> sectionBuilder)
    {
        var builder = SectionBuilder.Create();
        sectionBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddSection(string name, Action<ISectionBuilder> sectionBuilder)
    {
        var builder = SectionBuilder.Create().Name(name);
        sectionBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddSection(ISection section)
    {
        _builders.Add(() => section);
        return this;
    }

    public IResultsBuilder OK()
    {
        _results.OK = true;
        _isResultSet = true;
        return this;
    }

    public IResultsBuilder NOK()
    {
        _results.OK = false;
        _isResultSet = true;
        return this;
    }

    public IResultsBuilder Evaluate()
    {
        _evaluate = true;
        _isResultSet = true;
        return this;
    }

    public IResults Build()
    {
        foreach (var builder in _builders)
            _results.Add(builder());
        if (_evaluate)
            _results.OK = _results.Data.All(x => (x as IEvaluated)?.OK ?? true);
        if (!_isResultSet)
            throw new InvalidOperationException($"Result is not set. Use {nameof(OK)}, {nameof(NOK)} or {nameof(Evaluate)}");
        return _results;
    }
}
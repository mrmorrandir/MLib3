namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class ResultsBuilder : IResultsBuilder
{
    private readonly ISectionBuilderFactory _sectionBuilderFactory;
    private readonly IValueBuilderFactory _valueBuilderFactory;
    private readonly ICommentBuilderFactory _commentBuilderFactory;
    private readonly IInfoBuilderFactory _infoBuilderFactory;
    private readonly IFlagBuilderFactory _flagBuilderFactory;
    private readonly Results _results;
    private readonly List<Func<IElement>> _builders = new();
    private bool _isResultSet;
    private bool _evaluate;

    public ResultsBuilder(ISectionBuilderFactory sectionBuilderFactory, IValueBuilderFactory valueBuilderFactory, ICommentBuilderFactory commentBuilderFactory, IInfoBuilderFactory infoBuilderFactory, IFlagBuilderFactory flagBuilderFactory)
    {
        _sectionBuilderFactory = sectionBuilderFactory;
        _valueBuilderFactory = valueBuilderFactory;
        _commentBuilderFactory = commentBuilderFactory;
        _infoBuilderFactory = infoBuilderFactory;
        _flagBuilderFactory = flagBuilderFactory;
        _results = new Results();
    }
    
    public IResultsBuilder AddValue(Action<IValueBuilder> valueBuilder)
    {
        var builder = _valueBuilderFactory.Create();
        valueBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddComment(Action<ICommentBuilder> commentBuilder)
    {
        var builder = _commentBuilderFactory.Create();
        commentBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddFlag(Action<IFlagBuilder> flagBuilder)
    {
        var builder = _flagBuilderFactory.Create();
        flagBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddInfo(Action<IInfoBuilder> infoBuilder)
    {
        var builder = _infoBuilderFactory.Create();
        infoBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder AddSection(Action<ISectionBuilder> sectionBuilder)
    {
        var builder = _sectionBuilderFactory.Create();
        sectionBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public IResultsBuilder OK()
    {
        _results.Ok = true;
        _isResultSet = true;
        return this;
    }

    public IResultsBuilder NOK()
    {
        _results.Ok = false;
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
            _results.Ok = _results.Data.All(x => (x as IEvaluated)?.Ok ?? true);
        if (!_isResultSet)
            throw new InvalidOperationException($"Result is not set. Use {nameof(OK)}, {nameof(NOK)} or {nameof(Evaluate)}");
        return _results;
    }
}
namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class SectionBuilder : ISectionBuilder
{
    private readonly ISectionBuilderFactory _sectionBuilderFactory;
    private readonly IValueBuilderFactory _valueBuilderFactory;
    private readonly ICommentBuilderFactory _commentBuilderFactory;
    private readonly IFlagBuilderFactory _flagBuilderFactory;
    private readonly IInfoBuilderFactory _infoBuilderFactory;
    private readonly Section _section;
    private readonly List<Func<IElement>> _builders = new();
    private bool _isResultSet;
    private bool _evaluate;
    
    public SectionBuilder(ISectionBuilderFactory sectionBuilderFactory, IValueBuilderFactory valueBuilderFactory, ICommentBuilderFactory commentBuilderFactory, IFlagBuilderFactory flagBuilderFactory, IInfoBuilderFactory infoBuilderFactory, ISectionSetting? setting = null)
    {
        _sectionBuilderFactory = sectionBuilderFactory;
        _valueBuilderFactory = valueBuilderFactory;
        _commentBuilderFactory = commentBuilderFactory;
        _flagBuilderFactory = flagBuilderFactory;
        _infoBuilderFactory = infoBuilderFactory;
        _section = setting is null ? new Section() : new Section(setting);
    }
    
    public ISectionBuilder Name(string name)
    {
        _section.Name = name;
        return this;
    }

    public ISectionBuilder Description(string description)
    {
        _section.Description = description;
        return this;
    }

    public ISectionBuilder AddValue(Action<IValueBuilder> valueBuilder)
    {
        var builder = _valueBuilderFactory.Create();
        valueBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddComment(Action<ICommentBuilder> commentBuilder)
    {
        var builder = _commentBuilderFactory.Create();
        commentBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddFlag(Action<IFlagBuilder> flagBuilder)
    {
        var builder = _flagBuilderFactory.Create();
        flagBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddInfo(Action<IInfoBuilder> infoBuilder)
    {
        var builder = _infoBuilderFactory.Create();
        infoBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddSection(Action<ISectionBuilder> sectionBuilder)
    {
        var builder = _sectionBuilderFactory.Create();
        sectionBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }

    public ISectionBuilder OK()
    {
        _section.OK = true;
        _isResultSet = true;
        return this;
    }

    public ISectionBuilder NOK()
    {
        _section.OK = false;
        _isResultSet = true;
        return this;
    }

    public ISectionBuilder Evaluate()
    {
        _evaluate = true;
        _isResultSet = true;
        return this;
    }

    public ISection Build()
    {
        if (string.IsNullOrWhiteSpace(_section.Name))
            throw new InvalidOperationException($"{nameof(_section.Name)} is not set");
        foreach (var builder in _builders)
            _section.Add(builder());
        if (_evaluate)
            _section.OK = _section.Data.All(x => (x as IEvaluated)?.OK ?? true);
        if (!_isResultSet)
            throw new InvalidOperationException($"Result is not set. Use {nameof(OK)}, {nameof(NOK)} or {nameof(Evaluate)}");
        return _section;
    }
}
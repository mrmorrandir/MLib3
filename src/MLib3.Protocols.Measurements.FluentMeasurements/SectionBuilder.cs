namespace MLib3.Protocols.Measurements.FluentMeasurements;

public class SectionBuilder : ISectionBuilder
{
    private readonly Section _section;
    private readonly List<Func<IElement>> _builders = new();
    private bool _isResultSet;
    private bool _evaluate;
    
    private SectionBuilder(ISectionSetting? setting = null)
    {
        _section = setting is null ? new Section() : new Section(setting);
    }
    
    public static ISectionBuilder Create()
    {
        return new SectionBuilder();
    }

    public static ISectionBuilder CreateFromSetting(ISectionSetting setting)
    {
        if (setting == null) throw new ArgumentNullException(nameof(setting));
        return new SectionBuilder(setting);
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
        var builder = ValueBuilder.Create();
        valueBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddValue(string name, Action<IValueBuilder> valueBuilder)
    {
        var builder = ValueBuilder.Create().Name(name);
        valueBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }

    public ISectionBuilder AddValue(IValue value)
    {
        _builders.Add(() => value);
        return this;
    }
    
    public ISectionBuilder AddComment(Action<ICommentBuilder> commentBuilder)
    {
        var builder = CommentBuilder.Create();
        commentBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    public ISectionBuilder AddComment(string name, Action<ICommentBuilder> commentBuilder)
    {
        var builder = CommentBuilder.Create().Name(name);
        commentBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddComment(IComment comment)
    {
        _builders.Add(() => comment);
        return this;
    }
    
    public ISectionBuilder AddFlag(Action<IFlagBuilder> flagBuilder)
    {
        var builder = FlagBuilder.Create();
        flagBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddFlag(string name, Action<IFlagBuilder> flagBuilder)
    {
        var builder = FlagBuilder.Create().Name(name);
        flagBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddFlag(IFlag flag)
    {
        _builders.Add(() => flag);
        return this;
    }
    
    public ISectionBuilder AddInfo(Action<IInfoBuilder> infoBuilder)
    {
        var builder = InfoBuilder.Create();
        infoBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddInfo(string name, Action<IInfoBuilder> infoBuilder)
    {
        var builder = InfoBuilder.Create().Name(name);
        infoBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddInfo(IInfo info)
    {
        _builders.Add(() => info);
        return this;
    }
    
    public ISectionBuilder AddSection(Action<ISectionBuilder> sectionBuilder)
    {
        var builder = SectionBuilder.Create();
        sectionBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddSection(string name, Action<ISectionBuilder> sectionBuilder)
    {
        var builder = SectionBuilder.Create().Name(name);
        sectionBuilder(builder);
        _builders.Add(() => builder.Build());
        return this;
    }
    
    public ISectionBuilder AddSection(ISection section)
    {
        _builders.Add(() => section);
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
using MLib3.Localization.Interfaces;
using MLib3.Localization.Localizers.Json;
using MLib3.Localization.Localizers.Resx;

namespace MLib3.Localization;

public class CustomLocalizerBuilder : ICustomLocalizerBuilder
{
    private readonly IJsonLocalizerBuilderFactory _jsonLocalizerBuilderFactory;
    private readonly IResxLocalizerBuilderFactory _resxLocalizerBuilderFactory;
    private IJsonLocalizerBuilder? _jsonBuilder;
    private IResxLocalizerBuilder? _resxBuilder;

    public CustomLocalizerBuilder(IJsonLocalizerBuilderFactory jsonLocalizerBuilderFactory, IResxLocalizerBuilderFactory resxLocalizerBuilderFactory)
    {
        _jsonLocalizerBuilderFactory = jsonLocalizerBuilderFactory;
        _resxLocalizerBuilderFactory = resxLocalizerBuilderFactory;
    }
    
    public IJsonLocalizerConfig UseJson()
    {
        if (_resxBuilder is not null)
            throw new InvalidOperationException("The localizer type is already set.");
        _jsonBuilder = _jsonLocalizerBuilderFactory.Create();
        return _jsonBuilder;
    }

    public IResxLocalizerConfig UseResx()
    {
        if (_jsonBuilder is not null)
            throw new InvalidOperationException("The localizer type is already set.");
        _resxBuilder = _resxLocalizerBuilderFactory.Create();
        return _resxBuilder;
    }

    public ILocalizer Build()
    {
        if (_jsonBuilder is null && _resxBuilder is null)
            throw new InvalidOperationException("No Localizer type set.");
        if (_jsonBuilder is not null)
            return _jsonBuilder.Build();
        return _resxBuilder!.Build();
    }
}
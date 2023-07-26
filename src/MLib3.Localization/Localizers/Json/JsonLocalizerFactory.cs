using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Json;

public class JsonLocalizerFactory : IJsonLocalizerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JsonLocalizerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ILocalizer Create(TranslationDoc translationDoc, CultureInfo cultureInfo)
    {
        return ActivatorUtilities.CreateInstance<JsonLocalizer>(_serviceProvider, translationDoc, cultureInfo);
    }
}
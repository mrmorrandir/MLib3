using System.Globalization;
using Microsoft.Extensions.Logging;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Json;

public class JsonLocalizer : ILocalizer
{
    private readonly TranslationDoc _translationDoc;
    private readonly CultureInfo _cultureInfo;
    private readonly ILogger<JsonLocalizer>? _logger;

    public JsonLocalizer(TranslationDoc translationDoc, CultureInfo cultureInfo, ILogger<JsonLocalizer>? logger = null)
    {
        _translationDoc = translationDoc;
        _cultureInfo = cultureInfo;
        _logger = logger;
    }
    public string this[string key, params object?[]? args] => T(key, args);

    public string this[string key, CultureInfo cultureInfo, params object?[]? args] => T(key, cultureInfo, args);

    public string T(string key, params object?[]? args)
    {
        return T(key, _cultureInfo, args);
    }

    public string T(string key, CultureInfo cultureInfo, params object?[]? args)
    {
        try
        {
            var value = _translationDoc.Cultures.FirstOrDefault(c => c.Name == cultureInfo.Name)?.Strings.FirstOrDefault(t => t.Key == key)?.Value;
            if (value != null)
                return args == null || args.Length == 0 ? value : string.Format(value, args);
            _logger?.LogWarning("Key {key} not found", key);
            return $"[{key}]";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error while getting localized string with key '{key}'", key);
            return $"[{key}]";
        }
    }
}
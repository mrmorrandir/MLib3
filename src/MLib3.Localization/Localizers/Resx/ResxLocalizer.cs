using System.Globalization;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Logging;
using MLib3.Localization.Interfaces;

namespace MLib3.Localization.Localizers.Resx;

public class ResxLocalizer : ILocalizer
{
    private readonly CultureInfo _cultureInfo;
    private readonly ILogger<ResxLocalizer>? _logger;
    private readonly string _resource;
    private readonly ResourceManager _resourceManager;

    public ResxLocalizer(string resource, CultureInfo cultureInfo, Assembly? assembly = null,
        ILogger<ResxLocalizer>? logger = null)
    {
        _resource = resource;
        _cultureInfo = cultureInfo;
        _logger = logger;
        _resourceManager = new ResourceManager(resource, assembly ?? Assembly.GetCallingAssembly());
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
            var value = _resourceManager.GetString(key, cultureInfo);
            if (value != null)
                return args == null || args.Length == 0 ? value : string.Format(value, args);
            _logger?.LogWarning("Resource {Resource} not found for key {key}", _resource, key);
            return $"[{key}]";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error while getting localized string from '{resource}' with key '{key}'", _resource,
                key);
            return $"[{key}]";
        }
    }
}

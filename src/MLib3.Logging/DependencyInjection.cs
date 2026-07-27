using Microsoft.Extensions.DependencyInjection.Extensions;
using MLib3.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static partial class DependencyInjection
{
    /// <summary>
    /// Registers the shared logging payload sanitizer as a singleton.
    /// </summary>
    public static IServiceCollection AddLogPayloadSanitizer(
        this IServiceCollection services,
        Action<LogPayloadSanitizerOptions>? configureOptions = null)
    {
        var options = services.AddOptions<LogPayloadSanitizerOptions>();
        if (configureOptions is not null)
            options.Configure(configureOptions);

        services.TryAddSingleton<ILogPayloadSanitizer, LogPayloadSanitizer>();
        return services;
    }
}

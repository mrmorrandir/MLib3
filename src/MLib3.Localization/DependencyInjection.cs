using System.Dynamic;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using MLib3.Localization.Interfaces;
using MLib3.Localization.Localizers.Json;
using MLib3.Localization.Localizers.Resx;
using Microsoft.Extensions.Logging;

namespace MLib3.Localization;

public static class DependencyInjection
{
    // public static IServiceCollection AddJsonLocalizer(this IServiceCollection services, string filepath, CultureInfo cultureInfo)
    // {
    //     var translations = JsonSerializer.Deserialize<TranslationDoc>(File.ReadAllText("path/de-DE.json") + File.ReadAllText("path/en-US.json"));
    //     services.AddTransient<ILocalizer>(sp => ActivatorUtilities.CreateInstance<JsonLocalizer>(sp, translations!, cultureInfo));
    //     return services;
    // }
    // public static IServiceCollection AddResxLocalizer(this IServiceCollection services)
    // {
    //     services.AddTransient<ILocalizer, ResxLocalizer>();
    //     return services;
    // }

    public static IServiceCollection AddCustomLocalizer(this IServiceCollection services, Action<ICustomLocalizerConfig> configAction)
    {
        services.AddTransient<IJsonLocalizerBuilderFactory, JsonLocalizerBuilderFactory>();
        services.AddTransient<IResxLocalizerBuilderFactory, ResxLocalizerBuilderFactory>();
        services.AddTransient<IJsonLocalizerFactory, JsonLocalizerFactory>();
        services.AddTransient<IResxLocalizerFactory, ResxLocalizerFactory>();
        services.AddTransient<ICustomLocalizerBuilder, CustomLocalizerBuilder>();
        services.AddSingleton<ILocalizer>(sp =>
        {
            var builder = sp.GetRequiredService<ICustomLocalizerBuilder>();
            configAction(builder);
            return builder.Build();
        });
        return services;
    }
    
    public static void MyProgram()
    {
        var services = new ServiceCollection();

        services.AddCustomLocalizer(config =>
        {
            config
                .UseJson()
                .AddFile("./test.json")
                .SetCulture(new CultureInfo("en-US"));
        });
    }
}
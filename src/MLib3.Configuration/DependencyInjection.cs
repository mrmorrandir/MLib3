using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Configuration;

public static class DependencyInjection
{
    /// <summary>
    /// Use <see cref="AddSettings{TSettings}"/> to help with the usage of `appsettings.json` on console or windows applications, where you want to change the settings from the GUI.
    /// <para>
    ///    This method will add the following services:
    ///     <list type="bullet">
    ///         <item><see cref="ISettingsReader{TSettings}"/></item>
    ///         <item><see cref="ISettingsWriter{TSettings}"/></item>
    ///         <item><see cref="ISettingsManager"/></item>
    ///     </list>
    /// </para>
    /// <para>
    ///     In order to add the `appsettings.json` support into your application, you need to add the following code to your `Program.cs` or `App.xaml.cs` (here is an example from a WPF application front the `App_OnStartup` method):
    ///     <code>
    ///     var originalAppSettingsFileName = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "appsettings.json");
    ///     var appSettingsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetExecutingAssembly().GetName().Name, "appsettings.json");
    ///     var appSettingsFile = new FileInfo(originalAppSettingsFileName);
    ///     var file = new FileInfo(appSettingsFileName);
    ///     if (!file.Exists)
    ///     {
    ///         file.Directory.Create();
    ///         appSettingsFile.CopyTo(file.FullName);
    ///     }
    ///
    ///     var builder = Host.CreateApplicationBuilder();
    ///     // Add the additional appsettings.json file (which is copied from the original appsettings.json file) and
    ///     // which will overwrite the settings from the original appsettings.json file when loaded.
    ///     // This is the default behavior of the Microsoft.Extensions.Configuration.
    ///     builder.Configuration.AddJsonFile(appSettingsFileName, true);
    ///     </code>
    /// </para>
    /// </summary>
    /// <param name="services">A <see cref="IServiceCollection"/> for the DependencyInjection</param>
    /// <param name="settingsFilePath">The name of the file to use in addition to the `appsettings.json`</param>
    /// <typeparam name="TSettings"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddSettings<TSettings>(this IServiceCollection services, string settingsFilePath) where TSettings : class
    {
        services.AddSingleton<ISettingsReader<TSettings>>(provider => new SettingsReader<TSettings>(settingsFilePath));
        services.AddSingleton<ISettingsWriter<TSettings>>(provider => new SettingsWriter<TSettings>(settingsFilePath));
        services.AddSingleton<ISettingsManager, SettingsManager<TSettings>>();
        return services;
    }
}
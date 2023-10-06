using System.Globalization;

namespace MLib3.Localization.Interfaces;

public interface ILocalizer
{
    /// <summary>
    /// An indexer to get the localized string for the given key.
    /// </summary>
    /// <param name="key">The key of the string to localize</param>
    /// <param name="args">Arguments for the localized string</param>
    string this[string key, params object?[]? args] { get; }

    /// <summary>
    /// An indexer to get the localized string for the given key.
    /// </summary>
    /// <param name="key">The key of the string to localize</param>
    /// <param name="cultureInfo">The culture to localize to</param>
    /// <param name="args">Arguments for the localized string</param>
    string this[string key, CultureInfo cultureInfo, params object?[]? args] { get; }

    /// <summary>
    /// A method to get the localized string
    /// </summary>
    /// <param name="key">The key of the string to localize</param>
    /// <param name="args">Arguments for the localized string</param>
    /// <returns>The localized string</returns>
    string T(string key, params object?[]? args);

    /// <summary>
    /// A method to get the localized string
    /// </summary>
    /// <param name="key">The key of the string to localize</param>
    /// <param name="cultureInfo">The culture to localize to</param>
    /// <param name="args">Arguments for the localized string</param>
    /// <returns>The localized string</returns>
    string T(string key, CultureInfo cultureInfo, params object?[]? args);

    // CultureInfo GetCulture();
    // CultureInfo[] GetSupportedCultures();
}
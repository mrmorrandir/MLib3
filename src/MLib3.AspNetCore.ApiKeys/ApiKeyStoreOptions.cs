namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Provides the configured API keys used by the default <see cref="IApiKeyStore"/>.
/// </summary>
/// <remarks>
/// These options are bound from the <c>ApiKeys</c> configuration section by the default
/// authentication registration.
/// </remarks>
public class ApiKeyStoreOptions
{
    /// <summary>
    /// Gets or sets the configured API key entries.
    /// </summary>
    public ApiKeyEntry[] Keys { get; set; } = [];
}

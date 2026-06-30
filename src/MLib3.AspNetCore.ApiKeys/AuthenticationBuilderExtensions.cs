using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
///     Provides dependency injection extensions for API key authentication.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds API key authentication to the specified authentication builder.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="AuthenticationBuilder"/> to add the API key authentication scheme to.
    /// </param>
    /// <param name="configureApiKeys">
    /// A delegate to configure API key options, enabling customization of the API key configuration.
    /// This parameter is optional. If not provided, the API key store will be configured using
    /// application settings from the "ApiKeys" section.
    /// </param>
    /// <param name="configureAuthenticationOptions">
    /// A delegate to configure authentication scheme options, enabling advanced customization
    /// of API key authentication behavior. This parameter is optional. Default behaviors include:
    /// 1. Scheme name: "ApiKey".
    /// 2. API key permissions claim type: "permission".
    /// </param>
    /// <typeparam name="TApiKeyStore">
    /// The type of the API key store to use for managing API keys. This type must implement
    /// the <see cref="IApiKeyStore"/> interface.
    /// </typeparam>
    /// <returns>
    /// The <see cref="AuthenticationBuilder"/> to allow further authentication configuration.
    /// </returns>
    /// <remarks>
    /// When providing a custom API key store, the <typeparamref name="TApiKeyStore"/> must implement the <see cref="IApiKeyStore"/> interface.
    /// The "ApiKeys" section of the application's configuration can be left empty if the custom API key store is used, as the store will handle API key management independently.
    /// </remarks>
    public static AuthenticationBuilder AddApiKeyAuthentication<TApiKeyStore>(
        this AuthenticationBuilder builder,
        Action<ApiKeyStoreOptionsBuilder>? configureApiKeys = null,
        Action<ApiKeyAuthenticationOptions>? configureAuthenticationOptions = null) where TApiKeyStore : class, IApiKeyStore
    {
        if (configureApiKeys is null)
            builder.Services.AddOptions<ApiKeyStoreOptions>()
                .BindConfiguration("ApiKeys")
                .ValidateDataAnnotations()
                .ValidateOnStart();
        else
            builder.Services.AddOptions<ApiKeyStoreOptions>()
                .Configure(options =>
                {
                    var apiKeyBuilder = new ApiKeyStoreOptionsBuilder();
                    configureApiKeys(apiKeyBuilder);
                    var builtOptions = apiKeyBuilder.Build();

                    options.Keys = builtOptions.Keys;
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

        builder.Services.TryAddSingleton<IApiKeyStore, TApiKeyStore>();
        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationDefaults.SchemeName,
            configureAuthenticationOptions);
    }

    /// <summary>
    /// Adds API key authentication to the specified authentication builder.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="AuthenticationBuilder"/> to add the API key authentication scheme to.
    /// </param>
    /// <param name="configureApiKeys">
    /// A delegate to configure API key options, offering customization of the API key store configuration.
    /// This parameter is optional. If not provided, the API key store will be configured
    /// from the application's configuration ("ApiKeys" section).
    /// </param>
    /// <param name="configureAuthenticationOptions">
    /// A delegate to configure authentication scheme options, allowing advanced customization
    /// of the API key authentication behavior. This parameter is optional.
    /// Defaults include:
    /// 1. Scheme name: "ApiKey".
    /// 2. Header name: "X-Api-Key".
    /// 3. Claim type for API key permissions: "permission".
    /// </param>
    /// <returns>
    /// The <see cref="AuthenticationBuilder"/> to allow further configuration.
    /// </returns>
    public static AuthenticationBuilder AddApiKeyAuthentication(
        this AuthenticationBuilder builder,
        Action<ApiKeyStoreOptionsBuilder>? configureApiKeys = null,
        Action<ApiKeyAuthenticationOptions>? configureAuthenticationOptions = null) =>
        builder.AddApiKeyAuthentication<ApiKeyStore>(configureApiKeys, configureAuthenticationOptions);
}
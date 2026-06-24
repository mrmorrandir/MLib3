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
    /// The delegate to configure API key options, allowing customization of the API key store.
    /// This parameter is optional. When not provided, the API key store will be configured
    /// from the application's configuration ("ApiKeys" section).
    /// </param>
    /// <param name="configureAuthenticationOptions">
    /// The delegate to configure authentication scheme options, providing advanced customization
    /// of the API key authentication behavior. This parameter is optional.
    /// 1. The default scheme name is "ApiKey".
    /// 2. The default header name is "X-Api-Key".
    /// 3. The default claim type used for configured API key permissions is "permission".
    /// </param>
    /// <returns>
    /// The <see cref="AuthenticationBuilder"/> to allow further configuration.
    /// </returns>
    public static AuthenticationBuilder AddApiKeyAuthentication(
        this AuthenticationBuilder builder,
        Action<ApiKeyStoreOptionsBuilder>? configureApiKeys = null,
        Action<ApiKeyAuthenticationOptions>? configureAuthenticationOptions = null)
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

        builder.Services.TryAddSingleton<IApiKeyStore, ApiKeyStore>();
        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationDefaults.SchemeName,
            configureAuthenticationOptions);
    }
}
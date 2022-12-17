using Microsoft.Extensions.DependencyInjection;
using MLib3.AspDotNet.ApiKeys.Abstractions;
using MLib3.AspDotNet.ApiKeys.Generators;
using MLib3.AspDotNet.ApiKeys.Validators;

namespace MLib3.AspDotNet.ApiKeys;

public static class ApiKeysConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="IApiKeyGenerator"/> service.
    /// <para>
    /// This service is used to generate API keys. The default implementation uses the <see cref="RNGCryptoServiceProvider"/> to generate random bytes, and then converts them to a Base64 string with a length of 32 characters.
    /// The default implementation is registered as a singleton. If you want to use a different implementation, you can register it as a singleton or transient.
    /// </para>
    /// </summary>
    /// <param name="builder">An instance of the <see cref="ApiKeysConfigurationBuilder"/> to be used to register the required services</param>
    /// <param name="generatorBuilder">An optional <see cref="ApiKeyGeneratorBuilder"/> to configure the <see cref="IApiKeyGenerator"/></param>
    public static void UseApiKeyGenerator(this ApiKeysConfigurationBuilder builder, Action<ApiKeyGeneratorBuilder>? generatorBuilder = null)
    {
        var apiKeyGeneratorBuilder = new ApiKeyGeneratorBuilder();
        generatorBuilder?.Invoke(apiKeyGeneratorBuilder);
        builder.ConfigureServices(services => services.AddSingleton<IApiKeyGenerator>(apiKeyGeneratorBuilder.Build()));
    }
    
    /// <summary>
    /// Adds a <see cref="IApiKeyValidator"/> service.
    /// <para>
    /// This service is used to validate API keys. The default implementation compares the configured <paramref name="apiKey"/> with the API key that is passed to the <see cref="IApiKeyValidator.ValidateAsync"/> method.
    /// </para>
    /// </summary>
    /// <param name="builder">An instance of the <see cref="ApiKeysConfigurationBuilder"/> to be used to register the required services</param>
    /// <param name="apiKey">An API key for validation</param>
    public static void UseApiKey(this ApiKeysConfigurationBuilder builder, string apiKey)
    {
        builder.ConfigureServices(services => services.AddTransient<IApiKeyValidator>(sp => new StringApiKeyValidator(apiKey)));
    }
    
    /// <summary>
    /// Adds a <see cref="IApiKeyValidator"/> service.
    /// <para>
    /// This service is used to validate API keys. This implementation compares the API key that is found in the configured <paramref name="environmentVariable"/> with the API key that is passed to the <see cref="IApiKeyValidator.ValidateAsync"/> method. 
    /// </para>
    /// </summary>
    /// <param name="builder">An instance of the <see cref="ApiKeysConfigurationBuilder"/> to be used to register the required services</param>
    /// <param name="environmentVariable">The name of the environment variable where the API key is found.</param>
    public static void UseEnvironmentApiKey(this ApiKeysConfigurationBuilder builder, string environmentVariable = "HOST_API_KEY")
    {
        builder.ConfigureServices(services => services.AddTransient<IApiKeyValidator>(sp => new EnvironmentApiKeyValidator(environmentVariable)));
    }
}
using Microsoft.Extensions.Options;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Validates raw API keys against the configured <see cref="ApiKeyStoreOptions"/>.
/// </summary>
internal sealed class ApiKeyStore : IApiKeyStore
{
    private readonly ApiKeyStoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyStore"/> class.
    /// </summary>
    /// <param name="options">The configured API key store options.</param>
    public ApiKeyStore(IOptions<ApiKeyStoreOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public Task<Result<ApiKeyEntry>> ValidateAsync(string rawKey)
    {
        var hash = ApiKeyStoreOptionsBuilder.ComputeHash(rawKey);
        var entry = _options.Keys.FirstOrDefault(k =>
            string.Equals(k.HashedKey, hash, StringComparison.OrdinalIgnoreCase));

        if (entry is null)
            return Task.FromResult(Result.Fail<ApiKeyEntry>(new Error("Invalid API key.")));

        return Task.FromResult(Result.Ok(entry));
    }
}

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Validates raw API keys against configured API key entries.
/// </summary>
public interface IApiKeyStore
{
    /// <summary>
    /// Validates the given raw API key and returns the matching entry on success.
    /// </summary>
    /// <param name="rawKey">The raw API key from the incoming request.</param>
    /// <returns>A result containing the matching API key entry, or a failure when the key is invalid.</returns>
    Task<Result<ApiKeyEntry>> ValidateAsync(string rawKey);
}

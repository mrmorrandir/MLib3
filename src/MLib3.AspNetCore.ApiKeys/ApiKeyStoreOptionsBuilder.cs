using System.Security.Cryptography;
using System.Text;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Builds <see cref="ApiKeyStoreOptions"/> in code without using application configuration.
/// </summary>
public sealed class ApiKeyStoreOptionsBuilder
{
    private readonly List<ApiKeyEntry> _keys = [];

    /// <summary>
    /// Adds an API key from its raw value.
    /// </summary>
    /// <param name="name">The client name associated with the API key.</param>
    /// <param name="apiKey">The raw API key value. The value is hashed before it is stored.</param>
    /// <param name="permissions">The permissions granted to the API key.</param>
    /// <returns>The current builder.</returns>
    public ApiKeyStoreOptionsBuilder AddKey(
        string name,
        string apiKey,
        params string[] permissions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        return AddHashedKey(name, ComputeHash(apiKey), permissions);
    }

    /// <summary>
    /// Adds an API key from an existing SHA-256 hash.
    /// </summary>
    /// <param name="name">The client name associated with the API key.</param>
    /// <param name="hashedKey">The SHA-256 hash of the raw API key as lower-case or upper-case hex.</param>
    /// <param name="permissions">The permissions granted to the API key.</param>
    /// <returns>The current builder.</returns>
    public ApiKeyStoreOptionsBuilder AddHashedKey(
        string name,
        string hashedKey,
        params string[] permissions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedKey);

        _keys.Add(new ApiKeyEntry
        {
            Name = name,
            HashedKey = hashedKey,
            Permissions = permissions
        });

        return this;
    }

    internal ApiKeyStoreOptions Build()
    {
        return new ApiKeyStoreOptions
        {
            Keys = _keys.ToArray()
        };
    }

    internal static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}

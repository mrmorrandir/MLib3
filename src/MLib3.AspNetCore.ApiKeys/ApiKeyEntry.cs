using System.ComponentModel.DataAnnotations;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Represents a configured API key with its associated client name.
/// </summary>
public class ApiKeyEntry
{
    /// <summary>The human-readable client name used for logging.</summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>SHA-256 hash (lower-case hex) of the raw API key.</summary>
    [Required]
    public string HashedKey { get; set; } = string.Empty;

    /// <summary>Permissions granted to callers authenticated with this API key.</summary>
    public string[] Permissions { get; set; } = [];
}


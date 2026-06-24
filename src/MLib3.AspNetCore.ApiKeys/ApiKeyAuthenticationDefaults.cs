namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Defines default values used by API key authentication.
/// </summary>
public static class ApiKeyAuthenticationDefaults
{
    /// <summary>
    /// The default authentication scheme name.
    /// </summary>
    public const string SchemeName = "ApiKey";

    /// <summary>
    /// The default HTTP request header that contains the raw API key.
    /// </summary>
    public const string HeaderName = "X-Api-Key";

    /// <summary>
    /// The default claim type used for configured API key permissions.
    /// </summary>
    public const string PermissionClaimType = "permission";

    /// <summary>
    /// The configuration section used to bind <see cref="ApiKeyStoreOptions"/>.
    /// </summary>
    public const string ConfigurationSectionName = "ApiKeys";
}

using Microsoft.AspNetCore.Authentication;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Configures the API key authentication scheme.
/// </summary>
public sealed class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets the claim type used for permission claims created from matching API key entries.
    /// </summary>
    public string PermissionClaimType { get; set; } = ApiKeyAuthenticationDefaults.PermissionClaimType;
}

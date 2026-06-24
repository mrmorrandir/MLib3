using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Authenticates requests by validating an API key from the configured request header.
/// </summary>
public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IApiKeyStore _apiKeyStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="options">The monitor for authentication scheme options.</param>
    /// <param name="logger">The logger factory.</param>
    /// <param name="encoder">The URL encoder.</param>
    /// <param name="apiKeyStore">The API key store used to validate raw API keys.</param>
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyStore apiKeyStore) : base(options, logger, encoder)
    {
        _apiKeyStore = apiKeyStore;
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var hasHeader = Request.Headers.TryGetValue(ApiKeyAuthenticationDefaults.HeaderName, out var rawKey);
        if (!hasHeader)
            return AuthenticateResult.NoResult();

        var validation = await _apiKeyStore.ValidateAsync(rawKey.ToString());
        if (validation.IsFailed)
            return AuthenticateResult.Fail("Invalid API key.");

        var entry = validation.Value;

        var permissionClaimType = string.IsNullOrWhiteSpace(Options.PermissionClaimType)
            ? ApiKeyAuthenticationDefaults.PermissionClaimType
            : Options.PermissionClaimType;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, entry.Name)
        };

        claims.AddRange(entry.Permissions
            .Where(permission => !string.IsNullOrWhiteSpace(permission))
            .Distinct(StringComparer.Ordinal)
            .Select(permission => new Claim(permissionClaimType, permission)));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        Logger.LogInformation("API key authentication succeeded for client '{ClientName}'", entry.Name);
        return AuthenticateResult.Success(ticket);
    }

    /// <inheritdoc />
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }
}

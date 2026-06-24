# MLib3.AspNetCore.ApiKeys

API key authentication for ASP.NET Core applications.

## Configuration

The default registration binds API keys from the `ApiKeys` configuration section.

```json
{
  "ApiKeys": {
    "Keys": [
      {
        "Name": "internal-client",
        "HashedKey": "6d755f...",
        "Permissions": [ "orders:read", "orders:write" ]
      }
    ]
  }
}
```

`HashedKey` must be the SHA-256 hash of the raw API key encoded as hexadecimal text.

## Registration From Configuration

```csharp
builder.Services
    .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
    .AddApiKeyAuthentication();

builder.Services.AddAuthorization();
```

Incoming requests must include the API key in the `X-Api-Key` header.

## Registration With Builder

Use the builder overload when keys should be configured in code instead of `appsettings.json`.

```csharp
builder.Services
    .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
    .AddApiKeyAuthentication(apiKeys =>
    {
        apiKeys.AddKey("internal-client", "raw-api-key", "orders:read");
        apiKeys.AddHashedKey("worker", "6d755f...", "jobs:run");
    });
```

`AddKey` hashes the raw value before storing it in options. `AddHashedKey` accepts an existing SHA-256 hex hash.

## Scheme Options

The authentication scheme can be customized independently from the configured key store.

```csharp
builder.Services
    .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
    .AddApiKeyAuthentication(options =>
    {
        options.PermissionClaimType = "scope";
    });
```

When a key is valid, the handler creates a name claim from the configured entry name and one permission claim per configured permission.

## Swagger

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddApiKeyAuthentication();
});
```

## Complete Example

This example loads keys from `appsettings.json`, maps configured permissions to authorization policies, and protects a Minimal API endpoint with one of those policies.

```json
{
  "ApiKeys": {
    "Keys": [
      {
        "Name": "reporting-client",
        "HashedKey": "6d755f...",
        "Permissions": [ "deviations:view-all", "dashboard:view" ]
      }
    ]
  }
}
```

```csharp
using MLib3.AspNetCore.ApiKeys;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddApiKeyAuthentication();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("deviations:create-manual",
        policy => policy.RequireClaim("permission", "deviations:create-manual"));

    options.AddPolicy("deviations:view-own",
        policy => policy.RequireClaim("permission", "deviations:view-own"));

    options.AddPolicy("deviations:view-all",
        policy => policy.RequireClaim("permission", "deviations:view-all"));

    options.AddPolicy("deviations:review-small",
        policy => policy.RequireClaim("permission", "deviations:review-small"));

    options.AddPolicy("deviations:review-critical",
        policy => policy.RequireClaim("permission", "deviations:review-critical"));

    options.AddPolicy("dashboard:view",
        policy => policy.RequireClaim("permission", "dashboard:view"));

    options.AddPolicy("users:manage",
        policy => policy.RequireClaim("permission", "users:manage"));

    options.AddPolicy("roles:manage",
        policy => policy.RequireClaim("permission", "roles:manage"));

    options.AddPolicy("settings:manage",
        policy => policy.RequireClaim("permission", "settings:manage"));

    // This policy accepts either a signed-in cookie user or a valid API key.
    options.AddPolicy("data-analysis:read", policy =>
    {
        policy.AddAuthenticationSchemes(
            CookieAuthenticationDefaults.AuthenticationScheme,
            ApiKeyAuthenticationDefaults.SchemeName);

        policy.RequireClaim("permission", "deviations:view-all");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/deviations", async (
        [AsParameters] GetDeviationRecordsRequest request,
        IDeviationService deviationService,
        CancellationToken cancellationToken) =>
    {
        var result = await deviationService.GetAllAsync(
            isFullyProcessed: request.IsFullyProcessed,
            isApproved: request.IsApproved,
            isRejected: request.IsRejected,
            isCritical: request.IsCritical,
            isAcknowledgedByWorker: request.IsAcknowledgedByWorker,
            device: request.Device,
            serial: request.Serial,
            material: request.Material,
            from: request.From,
            to: request.To,
            offset: request.Offset,
            limit: request.Limit,
            cancellationToken: cancellationToken);

        if (result.IsFailed)
            return result.ToWebResult();

        var (items, total) = result.Value;
        return Results.Ok(new PagedResult<DeviationRecord>(
            items,
            total,
            request.Offset ?? 0,
            request.Limit ?? total));
    })
    .Produces<PagedResult<DeviationRecord>>()
    .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
    .WithTags("Deviations")
    .WithName("Deviations_GetAll")
    .WithSummary("Get deviation records")
    .WithDescription("Returns deviation records filtered by the provided optional query parameters.")
    .RequireAuthorization("data-analysis:read");

app.Run();
```

For larger applications, keep policy and permission names in constants to avoid typos. The important part is that the permission strings configured for an API key are emitted as `permission` claims and can then be required by authorization policies.

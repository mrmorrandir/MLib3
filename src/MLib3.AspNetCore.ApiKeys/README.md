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

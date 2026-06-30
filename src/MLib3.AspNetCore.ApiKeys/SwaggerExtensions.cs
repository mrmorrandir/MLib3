using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MLib3.AspNetCore.ApiKeys;

/// <summary>
/// Provides Swagger/OpenAPI extensions for API key authentication.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds the API key security definition to Swagger generation.
    /// </summary>
    /// <param name="options">The Swagger generator options.</param>
    public static void AddApiKeyAuthentication(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(ApiKeyAuthenticationDefaults.SchemeName, new OpenApiSecurityScheme
        {
            Name = ApiKeyAuthenticationDefaults.HeaderName,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "API key authentication via 'X-Api-Key' Header"
        });
    }
}

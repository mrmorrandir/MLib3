using AwesomeAssertions;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MLib3.AspNetCore.ApiKeys;

namespace MLib3.AspNetCore.ApiKeys.UnitTests;

public class AuthenticationBuilderExtensionsTests
{
    [Fact]
    public async Task AddApiKeyAuthentication_WithConfiguredKeys_RegistersStoreThatValidatesRawApiKeys()
    {
        var services = new ServiceCollection();

        services
            .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
            .AddApiKeyAuthentication(apiKeys =>
            {
                apiKeys.AddKey("internal-client", "raw-api-key", "orders:read");
            });

        var serviceProvider = services.BuildServiceProvider();
        var store = serviceProvider.GetRequiredService<IApiKeyStore>();

        var validResult = await store.ValidateAsync("raw-api-key");
        var invalidResult = await store.ValidateAsync("wrong-key");

        validResult.IsSuccess.Should().BeTrue();
        validResult.Value.Name.Should().Be("internal-client");
        validResult.Value.Permissions.Should().ContainSingle().Which.Should().Be("orders:read");
        invalidResult.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task AddApiKeyAuthentication_WithConfiguration_RegistersStoreFromApiKeysSection()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiKeys:Keys:0:Name"] = "configured-client",
                ["ApiKeys:Keys:0:HashedKey"] = "bcd5f900eb42c934378559f8ef78d383a3bdd194a178f47c2b6ed3f6b9ec9127",
                ["ApiKeys:Keys:0:Permissions:0"] = "dashboard:view"
            })
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);

        services
            .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
            .AddApiKeyAuthentication();

        var serviceProvider = services.BuildServiceProvider();
        var store = serviceProvider.GetRequiredService<IApiKeyStore>();

        var result = await store.ValidateAsync("configured-api-key");

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("configured-client");
        result.Value.Permissions.Should().ContainSingle().Which.Should().Be("dashboard:view");
    }

    [Fact]
    public void AddApiKeyAuthentication_WithAuthenticationOptions_ConfiguresSchemeOptions()
    {
        var services = new ServiceCollection();

        services
            .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
            .AddApiKeyAuthentication(
                configureAuthenticationOptions: options =>
                {
                    options.PermissionClaimType = "scope";
                });

        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider
            .GetRequiredService<IOptionsMonitor<ApiKeyAuthenticationOptions>>()
            .Get(ApiKeyAuthenticationDefaults.SchemeName);

        options.PermissionClaimType.Should().Be("scope");
    }

    [Fact]
    public void AddApiKeyAuthentication_WithCustomStore_RegistersCustomStore()
    {
        var services = new ServiceCollection();

        services
            .AddAuthentication(ApiKeyAuthenticationDefaults.SchemeName)
            .AddApiKeyAuthentication<AlwaysValidApiKeyStore>();

        var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<IApiKeyStore>()
            .Should().BeOfType<AlwaysValidApiKeyStore>();
    }

    private sealed class AlwaysValidApiKeyStore : IApiKeyStore
    {
        public Task<Result<ApiKeyEntry>> ValidateAsync(string rawKey) =>
            Task.FromResult(Result.Ok(new ApiKeyEntry
            {
                Name = "custom"
            }));
    }
}

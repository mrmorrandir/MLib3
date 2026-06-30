using System.Security.Claims;
using System.Text.Encodings.Web;
using AwesomeAssertions;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MLib3.AspNetCore.ApiKeys;

namespace MLib3.AspNetCore.ApiKeys.UnitTests;

public class ApiKeyAuthenticationHandlerTests
{
    [Fact]
    public async Task AuthenticateAsync_WhenHeaderIsMissing_ReturnsNoResult()
    {
        var handler = CreateHandler(FakeApiKeyStore.Invalid());
        await InitializeAsync(handler);

        var result = await handler.AuthenticateAsync();

        result.None.Should().BeTrue();
    }

    [Fact]
    public async Task AuthenticateAsync_WhenApiKeyIsInvalid_ReturnsFailure()
    {
        var store = FakeApiKeyStore.Invalid();
        var handler = CreateHandler(store);
        var context = await InitializeAsync(handler);
        context.Request.Headers[ApiKeyAuthenticationDefaults.HeaderName] = "invalid-key";

        var result = await handler.AuthenticateAsync();

        result.Succeeded.Should().BeFalse();
        result.Failure.Should().NotBeNull();
        result.Failure!.Message.Should().Be("Invalid API key.");
        store.ValidatedRawKeys.Should().ContainSingle().Which.Should().Be("invalid-key");
    }

    [Fact]
    public async Task AuthenticateAsync_WhenApiKeyIsValid_ReturnsPrincipalWithNameAndDistinctPermissionClaims()
    {
        var entry = new ApiKeyEntry
        {
            Name = "reporting-client",
            Permissions = ["orders:read", "orders:read", "", " ", "orders:write"]
        };
        var store = FakeApiKeyStore.Valid(entry);
        var handler = CreateHandler(store);
        var context = await InitializeAsync(handler);
        context.Request.Headers[ApiKeyAuthenticationDefaults.HeaderName] = "valid-key";

        var result = await handler.AuthenticateAsync();

        result.Succeeded.Should().BeTrue();
        result.Principal!.Identity!.AuthenticationType.Should().Be(ApiKeyAuthenticationDefaults.SchemeName);
        result.Principal.Identity.Name.Should().Be("reporting-client");
        result.Principal.FindAll(ApiKeyAuthenticationDefaults.PermissionClaimType)
            .Select(x => x.Value)
            .Should().BeEquivalentTo(["orders:read", "orders:write"]);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenPermissionClaimTypeIsConfigured_UsesConfiguredClaimType()
    {
        var entry = new ApiKeyEntry
        {
            Name = "worker",
            Permissions = ["jobs:run"]
        };
        var handler = CreateHandler(
            FakeApiKeyStore.Valid(entry),
            new ApiKeyAuthenticationOptions
            {
                PermissionClaimType = "scope"
            });
        var context = await InitializeAsync(handler);
        context.Request.Headers[ApiKeyAuthenticationDefaults.HeaderName] = "valid-key";

        var result = await handler.AuthenticateAsync();

        result.Succeeded.Should().BeTrue();
        result.Principal!.FindFirst("scope")!.Value.Should().Be("jobs:run");
        result.Principal.FindFirst(ApiKeyAuthenticationDefaults.PermissionClaimType).Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task AuthenticateAsync_WhenPermissionClaimTypeIsBlank_UsesDefaultClaimType(string configuredClaimType)
    {
        var entry = new ApiKeyEntry
        {
            Name = "worker",
            Permissions = ["jobs:run"]
        };
        var handler = CreateHandler(
            FakeApiKeyStore.Valid(entry),
            new ApiKeyAuthenticationOptions
            {
                PermissionClaimType = configuredClaimType
            });
        var context = await InitializeAsync(handler);
        context.Request.Headers[ApiKeyAuthenticationDefaults.HeaderName] = "valid-key";

        var result = await handler.AuthenticateAsync();

        result.Succeeded.Should().BeTrue();
        result.Principal!.FindFirst(ApiKeyAuthenticationDefaults.PermissionClaimType)!.Value.Should().Be("jobs:run");
    }

    [Fact]
    public async Task ChallengeAsync_SetsUnauthorizedStatusCode()
    {
        var handler = CreateHandler(FakeApiKeyStore.Invalid());
        var context = await InitializeAsync(handler);

        await handler.ChallengeAsync(new AuthenticationProperties());

        context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task ForbidAsync_SetsForbiddenStatusCode()
    {
        var handler = CreateHandler(FakeApiKeyStore.Valid(new ApiKeyEntry
        {
            Name = "client"
        }));
        var context = await InitializeAsync(handler);

        await handler.ForbidAsync(new AuthenticationProperties());

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    private static ApiKeyAuthenticationHandler CreateHandler(
        IApiKeyStore store,
        ApiKeyAuthenticationOptions? options = null) =>
        new(
            new TestOptionsMonitor<ApiKeyAuthenticationOptions>(
                options ?? new ApiKeyAuthenticationOptions()),
            NullLoggerFactory.Instance,
            UrlEncoder.Default,
            store);

    private static async Task<DefaultHttpContext> InitializeAsync(ApiKeyAuthenticationHandler handler)
    {
        var context = new DefaultHttpContext();
        var scheme = new AuthenticationScheme(
            ApiKeyAuthenticationDefaults.SchemeName,
            ApiKeyAuthenticationDefaults.SchemeName,
            typeof(ApiKeyAuthenticationHandler));

        await handler.InitializeAsync(scheme, context);
        return context;
    }

    private sealed class TestOptionsMonitor<TOptions>(TOptions currentValue) : IOptionsMonitor<TOptions>
    {
        public TOptions CurrentValue => currentValue;

        public TOptions Get(string? name) => currentValue;

        public IDisposable? OnChange(Action<TOptions, string?> listener) => null;
    }

    private sealed class FakeApiKeyStore(Result<ApiKeyEntry> result) : IApiKeyStore
    {
        public List<string> ValidatedRawKeys { get; } = [];

        public static FakeApiKeyStore Valid(ApiKeyEntry entry) => new(Result.Ok(entry));

        public static FakeApiKeyStore Invalid() => new(Result.Fail<ApiKeyEntry>("Invalid API key."));

        public Task<Result<ApiKeyEntry>> ValidateAsync(string rawKey)
        {
            ValidatedRawKeys.Add(rawKey);
            return Task.FromResult(result);
        }
    }
}

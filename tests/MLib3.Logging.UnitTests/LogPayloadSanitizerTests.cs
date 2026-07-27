using System.Text.Json;
using System.Text.Json.Serialization;
using AwesomeAssertions;
using FluentResults;
using Microsoft.Extensions.Options;

namespace MLib3.Logging.UnitTests;

public class LogPayloadSanitizerTests
{
    [Fact]
    public void Sanitize_ShouldRedactString_WhenPropertyHasAttribute()
    {
        // Arrange
        var sanitizer = CreateSanitizer();
        var payload = new Credentials("visible-user", "password-unique-1");

        // Act
        var result = sanitizer.Sanitize(payload);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().Contain("\"password\":\"[REDACTED]\"");
        result.Payload.Should().NotContain("password-unique-1");
    }

    [Fact]
    public void Sanitize_ShouldUseCustomReplacement_WhenConfiguredOnAttribute()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.Sanitize(new CustomCredentials("secret-unique-2"));

        // Assert
        result.Payload.Should().Contain("\"pin\":\"***\"");
        result.Payload.Should().NotContain("secret-unique-2");
    }

    [Fact]
    public void Sanitize_ShouldRemoveProperty_WhenPropertyIsIgnored()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.Sanitize(new IgnoredPropertyPayload("visible", "ignored-unique-3"));

        // Assert
        result.Payload.Should().Contain("visible");
        result.Payload.Should().NotContain("ignored-unique-3");
        result.Payload.Should().NotContain("internalValue");
    }

    [Fact]
    public void Sanitize_ShouldExcludePayload_WhenTypeIsIgnored()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.Sanitize(new IgnoredPayload("ignored-unique-4"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsExcluded.Should().BeTrue();
        result.Payload.Should().BeNull();
    }

    [Fact]
    public void Sanitize_ShouldRedactNestedProperties()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.Sanitize(new NestedPayload(new Credentials("Ada", "nested-unique-5")));

        // Assert
        result.Payload.Should().Contain("Ada");
        result.Payload.Should().Contain("[REDACTED]");
        result.Payload.Should().NotContain("nested-unique-5");
    }

    [Fact]
    public void Sanitize_ShouldRedactPropertiesInCollections()
    {
        // Arrange
        var sanitizer = CreateSanitizer();
        var payload = new[] { new Credentials("Ada", "collection-unique-6") };

        // Act
        var result = sanitizer.Sanitize(payload);

        // Assert
        result.Payload.Should().Contain("[REDACTED]");
        result.Payload.Should().NotContain("collection-unique-6");
    }

    [Fact]
    public void Sanitize_ShouldRedactPropertiesInDictionaries()
    {
        // Arrange
        var sanitizer = CreateSanitizer();
        var payload = new Dictionary<string, Credentials>
        {
            ["account"] = new Credentials("Ada", "dictionary-unique-7")
        };

        // Act
        var result = sanitizer.Sanitize(payload);

        // Assert
        result.Payload.Should().Contain("[REDACTED]");
        result.Payload.Should().NotContain("dictionary-unique-7");
    }

    [Fact]
    public void Sanitize_ShouldRedactSuccessfulResultValue()
    {
        // Arrange
        var sanitizer = CreateSanitizer();
        var payload = Result.Ok(new TokenPayload("jwt-unique-8", DateTimeOffset.UnixEpoch));

        // Act
        var result = sanitizer.Sanitize(payload);

        // Assert
        result.Payload.Should().Contain("\"value\"");
        result.Payload.Should().Contain("[REDACTED]");
        result.Payload.Should().NotContain("jwt-unique-8");
    }

    [Fact]
    public void Sanitize_ShouldNotReadValue_WhenResultIsFailed()
    {
        // Arrange
        var sanitizer = CreateSanitizer();
        var payload = Result.Fail<ThrowingPayload>("failed");

        // Act
        var result = sanitizer.Sanitize(payload);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().Contain("\"isFailed\":true");
        result.Payload.Should().NotContain("\"value\"");
    }

    [Fact]
    public void Sanitize_ShouldRedactSensitiveErrorMetadata()
    {
        // Arrange
        var sanitizer = CreateSanitizer();
        var error = new Error("Authentication failed")
            .WithMetadata("token", "metadata-jwt-unique-10")
            .WithMetadata("context", new Credentials("Ada", "metadata-password-unique-10"));
        var payload = Result.Fail(error);

        // Act
        var result = sanitizer.Sanitize(payload);

        // Assert
        result.Payload.Should().Contain("[REDACTED]");
        result.Payload.Should().NotContain("metadata-jwt-unique-10");
        result.Payload.Should().NotContain("metadata-password-unique-10");
    }

    [Fact]
    public void Sanitize_ShouldUseJsonPropertyName()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.Sanitize(new NamedPayload("named-unique-11"));

        // Assert
        result.Payload.Should().Contain("\"credential_value\":\"[REDACTED]\"");
        result.Payload.Should().NotContain("named-unique-11");
    }

    [Fact]
    public void Sanitize_ShouldUseConfiguredNamingPolicy()
    {
        // Arrange
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var sanitizer = CreateSanitizer(serializerOptions);

        // Act
        var result = sanitizer.Sanitize(new NamingPayload("visible"));

        // Assert
        result.Payload.Should().Contain("\"display_name\":\"visible\"");
    }

    [Fact]
    public void Sanitize_ShouldFailClosed_WhenPropertyGetterThrows()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.Sanitize(new ThrowingPayload());

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Payload.Should().BeNull();
    }

    [Fact]
    public void SanitizeJson_ShouldMatchPropertiesCaseInsensitively()
    {
        // Arrange
        var sanitizer = CreateSanitizer();

        // Act
        var result = sanitizer.SanitizeJson(
            """{"USERNAME":"Ada","PASSWORD":"json-password-unique-14"}""",
            typeof(Credentials));

        // Assert
        result.Payload.Should().Contain("[REDACTED]");
        result.Payload.Should().NotContain("json-password-unique-14");
    }

    private static LogPayloadSanitizer CreateSanitizer(JsonSerializerOptions? serializerOptions = null)
    {
        return new LogPayloadSanitizer(Options.Create(new LogPayloadSanitizerOptions
        {
            SerializerOptions = serializerOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web)
        }));
    }

    private sealed record Credentials(string Username, [property: LogRedact] string Password);

    private sealed record CustomCredentials([property: LogRedact("***")] string Pin);

    private sealed record IgnoredPropertyPayload(
        string Name,
        [property: LogPayloadIgnore] string InternalValue);

    [LogPayloadIgnore]
    private sealed record IgnoredPayload(string Value);

    private sealed record NestedPayload(Credentials Credentials);

    private sealed record TokenPayload(
        [property: LogRedact] string Token,
        DateTimeOffset ValidUntil);

    private sealed record NamedPayload(
        [property: JsonPropertyName("credential_value"), LogRedact] string Value);

    private sealed record NamingPayload(string DisplayName);

    private sealed class ThrowingPayload
    {
        public string Value => throw new InvalidOperationException("Getter failed.");
    }
}

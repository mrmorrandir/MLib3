using AwesomeAssertions;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;
using MLib3.AspNetCore.Application.Behaviours;
using MLib3.Logging;
using Microsoft.Extensions.Options;

namespace MLib3.AspNetCore.Application.UnitTests.Behaviours;

public class LoggingBehaviourTests
{
    [Fact]
    public async Task Handle_ShouldLogInformation_WhenResponseIsSuccessful()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<LoggingBehaviour<TestCommand, Result>>();
        var behaviour = new LoggingBehaviour<TestCommand, Result>(logger, CreateSanitizer());
        var request = new TestCommand("Create");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => ValueTask.FromResult(Result.Ok()),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.LogLevel == LogLevel.Information
            && entry.Message.Contains("Request:")
            && entry.Message.Contains("Response:"));
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenResponseIsFailed()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<LoggingBehaviour<TestCommand, Result>>();
        var behaviour = new LoggingBehaviour<TestCommand, Result>(logger, CreateSanitizer());
        var request = new TestCommand("Create");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => ValueTask.FromResult(Result.Fail("Failed")),
            CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.LogLevel == LogLevel.Warning
            && entry.Message.Contains("Request:")
            && entry.Message.Contains("Response:"));
    }

    [Fact]
    public async Task Handle_ShouldNotLogCredentials_WhenRequestAndResultContainSensitiveValues()
    {
        // Arrange
        const string password = "behaviour-password-unique-15";
        const string token = "behaviour-jwt-unique-15";
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<LoggingBehaviour<AuthCommand, Result<AuthResult>>>();
        var behaviour = new LoggingBehaviour<AuthCommand, Result<AuthResult>>(logger, CreateSanitizer());
        var request = new AuthCommand("Ada", password);

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => ValueTask.FromResult(Result.Ok(new AuthResult(token, DateTimeOffset.UnixEpoch))),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var messages = string.Join(Environment.NewLine, loggerProvider.Entries.Select(entry => entry.Message));
        messages.Should().Contain("Ada");
        messages.Should().Contain("[REDACTED]");
        messages.Should().NotContain(password);
        messages.Should().NotContain(token);
    }

    private static LogPayloadSanitizer CreateSanitizer()
    {
        return new LogPayloadSanitizer(Options.Create(new LogPayloadSanitizerOptions()));
    }

    private sealed record TestCommand(string Name) : ICommand<Result>;

    private sealed record AuthCommand(
        string Username,
        [property: LogRedact] string Password) : ICommand<Result<AuthResult>>;

    private sealed record AuthResult(
        [property: LogRedact] string Token,
        DateTimeOffset ValidUntil);
}

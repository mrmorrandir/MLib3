using AwesomeAssertions;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;
using MLib3.AspNetCore.Application.Behaviours;

namespace MLib3.AspNetCore.Application.UnitTests.Behaviours;

public class LoggingBehaviourTests
{
    [Fact]
    public async Task Handle_ShouldLogInformation_WhenResponseIsSuccessful()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<LoggingBehaviour<TestCommand, Result>>();
        var behaviour = new LoggingBehaviour<TestCommand, Result>(logger);
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
        var behaviour = new LoggingBehaviour<TestCommand, Result>(logger);
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

    private sealed record TestCommand(string Name) : ICommand<Result>;
}

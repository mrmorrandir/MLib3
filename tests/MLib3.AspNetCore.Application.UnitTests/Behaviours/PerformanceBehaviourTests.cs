using AwesomeAssertions;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;
using MLib3.AspNetCore.Application.Behaviours;

namespace MLib3.AspNetCore.Application.UnitTests.Behaviours;

public class PerformanceBehaviourTests
{
    [Fact]
    public async Task Handle_ShouldLogDebug_WhenElapsedTimeDoesNotExceedThreshold()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<PerformanceBehaviour<TestCommand, Result>>();
        var behaviour = new PerformanceBehaviour<TestCommand, Result>(logger, int.MaxValue);
        var request = new TestCommand("Create");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => ValueTask.FromResult(Result.Ok()),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.LogLevel == LogLevel.Debug
            && entry.Message.Contains(nameof(TestCommand)));
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenElapsedTimeExceedsThreshold()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<PerformanceBehaviour<TestCommand, Result>>();
        var behaviour = new PerformanceBehaviour<TestCommand, Result>(logger, -1);
        var request = new TestCommand("Create");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => ValueTask.FromResult(Result.Ok()),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.LogLevel == LogLevel.Warning
            && entry.Message.Contains("Long Running Request")
            && entry.Message.Contains(nameof(TestCommand)));
    }

    private sealed record TestCommand(string Name) : ICommand<Result>;
}

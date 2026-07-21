using AwesomeAssertions;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;
using MLib3.AspNetCore.Application.Behaviours;

namespace MLib3.AspNetCore.Application.UnitTests.Behaviours;

public class UnhandledExceptionBehaviourTests
{
    [Fact]
    public async Task Handle_ShouldReturnNextResult_WhenNextCompletes()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<UnhandledExceptionBehaviour<TestCommand, Result>>();
        var behaviour = new UnhandledExceptionBehaviour<TestCommand, Result>(logger);
        var request = new TestCommand("Create");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => ValueTask.FromResult(Result.Ok()),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        loggerProvider.Entries.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenNonGenericResultHandlerThrows()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<UnhandledExceptionBehaviour<TestCommand, Result>>();
        var behaviour = new UnhandledExceptionBehaviour<TestCommand, Result>(logger);
        var request = new TestCommand("Create");
        var exception = new InvalidOperationException("Handler failed.");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => throw exception,
            CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        var error = result.Errors.Should().ContainSingle().Subject;
        var exceptionalError = error.Should().BeOfType<ExceptionalError>().Subject;
        exceptionalError.Exception.Should().BeSameAs(exception);
        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.LogLevel == LogLevel.Error
            && entry.Exception == exception
            && entry.Message.Contains(nameof(TestCommand)));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenGenericResultHandlerThrows()
    {
        // Arrange
        var loggerProvider = new CapturingLoggerProvider();
        var logger = loggerProvider.CreateLogger<UnhandledExceptionBehaviour<TestQuery, Result<string>>>();
        var behaviour = new UnhandledExceptionBehaviour<TestQuery, Result<string>>(logger);
        var request = new TestQuery("Find");
        var exception = new InvalidOperationException("Handler failed.");

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) => throw exception,
            CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        var error = result.Errors.Should().ContainSingle().Subject;
        var exceptionalError = error.Should().BeOfType<ExceptionalError>().Subject;
        exceptionalError.Exception.Should().BeSameAs(exception);
        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.LogLevel == LogLevel.Error
            && entry.Exception == exception
            && entry.Message.Contains(nameof(TestQuery)));
    }

    private sealed record TestCommand(string Name) : ICommand<Result>;

    private sealed record TestQuery(string Name) : IQuery<Result<string>>;
}

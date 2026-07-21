using AwesomeAssertions;
using FluentResults;
using FluentValidation;
using Mediator;
using MLib3.AspNetCore.Application.Behaviours;

namespace MLib3.AspNetCore.Application.UnitTests.Behaviours;

public class ValidationBehaviourTests
{
    [Fact]
    public async Task Handle_ShouldCallNext_WhenNoValidatorsAreRegistered()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<TestCommand, Result>([]);
        var request = new TestCommand("Create");
        var wasNextCalled = false;

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) =>
            {
                wasNextCalled = true;
                return ValueTask.FromResult(Result.Ok());
            },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        wasNextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldCallNext_WhenValidationSucceeds()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<TestCommand, Result>([new TestCommandValidator()]);
        var request = new TestCommand("Create");
        var wasNextCalled = false;

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) =>
            {
                wasNextCalled = true;
                return ValueTask.FromResult(Result.Ok());
            },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        wasNextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenNonGenericResultValidationFails()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<TestCommand, Result>([new TestCommandValidator()]);
        var request = new TestCommand(string.Empty);
        var wasNextCalled = false;

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) =>
            {
                wasNextCalled = true;
                return ValueTask.FromResult(Result.Ok());
            },
            CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        wasNextCalled.Should().BeFalse();
        result.Errors.Should().ContainSingle(error =>
            error.Message == "Validation"
            && error.Reasons.Any(reason => reason.Message == "Name is required."));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenGenericResultValidationFails()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<TestQuery, Result<string>>([new TestQueryValidator()]);
        var request = new TestQuery(string.Empty);
        var wasNextCalled = false;

        // Act
        var result = await behaviour.Handle(
            request,
            (_, _) =>
            {
                wasNextCalled = true;
                return ValueTask.FromResult(Result.Ok("Created"));
            },
            CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        wasNextCalled.Should().BeFalse();
        result.Errors.Should().ContainSingle(error =>
            error.Message == "Validation"
            && error.Reasons.Any(reason => reason.Message == "Name is required."));
    }

    private sealed record TestCommand(string Name) : ICommand<Result>;

    private sealed record TestQuery(string Name) : IQuery<Result<string>>;

    private sealed class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .WithMessage("Name is required.");
        }
    }

    private sealed class TestQueryValidator : AbstractValidator<TestQuery>
    {
        public TestQueryValidator()
        {
            RuleFor(query => query.Name)
                .NotEmpty()
                .WithMessage("Name is required.");
        }
    }
}

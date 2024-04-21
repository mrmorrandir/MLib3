using System.Windows.Input;

namespace MLib3.MVVM.UnitTests;

public class AsyncCommandTests
{
    [Fact]
    public void CanExecute_ShouldReturnTrue_WhenPredicateIsTrue()
    {
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => { });

        var canExecute = command.CanExecute();

        canExecute.Should().BeTrue();
    }

    [Fact]
    public void CanExecute_ShouldReturnFalse_WhenPredicateIsFalse()
    {
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => false, _ => { });

        var canExecute = command.CanExecute();

        canExecute.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallAction_WhenCanExecuteIsTrue()
    {
        var executed = false;
        var command = new AsyncCommand<object>(_ => { executed = true; return Task.CompletedTask; }, _ => true, _ => { });

        await command.ExecuteAsync();

        executed.Should().BeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotCallAction_WhenCanExecuteIsFalse()
    {
        var executed = false;
        var command = new AsyncCommand<object>(_ => { executed = true; return Task.CompletedTask; }, _ => false, _ => { });

        await command.ExecuteAsync();

        executed.Should().BeFalse();
    }

    [Fact]
    public void CanExecuteChanged_ShouldBeRaised_WhenRaiseCanExecuteChangedIsCalled()
    {
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => { });
        var raised = false;
        command.CanExecuteChanged += (s, e) => raised = true;

        command.RaiseCanExecuteChanged();

        raised.Should().BeTrue();
    }

    [Fact]
    public void ExecuteAsync_ShouldBeCalled_WhenICommandExecuteIsCalled()
    {
        var executed = false;
        var command = new AsyncCommand<object>(_ => { executed = true; return Task.CompletedTask; }, _ => true, _ => { });

        ((ICommand)command).Execute(null);

        executed.Should().BeTrue();
    }

    [Fact]
    public void CanExecute_ShouldBeCalled_WhenICommandCanExecuteIsCalled()
    {
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => { });
        var canExecute = ((ICommand)command).CanExecute(null);

        canExecute.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_ShouldNotThrow_WhenParameterIsNull()
    {
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => {  });

        var act = () => command.ExecuteAsync();

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public void CanExecute_ShouldNotThrow_WhenParameterIsNull()
    {
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => { });

        Action act = () => command.CanExecute();

        act.Should().NotThrow();
    }

    [Fact]
    public void Execute_ShouldNotThrow_WhenICommandExecuteIsCalledWithNull()
    {
        var errorCallbackCalled = false;
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => errorCallbackCalled = true);

        Action act = () => ((ICommand)command).Execute(null);

        act.Should().NotThrow();
        errorCallbackCalled.Should().BeFalse();
    }

    [Fact]
    public void CanExecute_ShouldNotThrow_WhenICommandCanExecuteIsCalledWithNull()
    {
        var errorCallbackCalled = false;
        var command = new AsyncCommand<object>(_ => Task.CompletedTask, _ => true, _ => errorCallbackCalled = true);

        Action act = () => ((ICommand)command).CanExecute(null);

        act.Should().NotThrow();
        errorCallbackCalled.Should().BeFalse();
    }
    
    [Fact]
    public void Execute_ShouldCallErrorCallback_WhenErrorOccurs()
    {
        var exception = new Exception();
        var errorCallbackCalled = false;
        var command = new AsyncCommand<object>(_ => throw exception, _ => true, _ => errorCallbackCalled = true);

        ((ICommand)command).Execute(null);

        errorCallbackCalled.Should().BeTrue();
    }
}
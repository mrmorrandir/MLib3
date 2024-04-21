using System.Windows.Input;

namespace MLib3.MVVM.UnitTests;

public class DelegateCommandTests
{
    [Fact]
    public void CanExecute_ShouldReturnTrue_WhenPredicateIsTrue()
    {
        var command = new DelegateCommand(_ => { }, _ => true);

        var canExecute = command.CanExecute();

        canExecute.Should().BeTrue();
    }
    
    [Fact]
    public void CanExecute_ShouldReturnFalse_WhenPredicateIsFalse()
    {
        var command = new DelegateCommand(_ => { }, _ => false);

        var canExecute = command.CanExecute();

        canExecute.Should().BeFalse();
    }
    
    [Fact]
    public void Execute_ShouldCallAction_WhenCanExecuteIsTrue()
    {
        var executed = false;
        var command = new DelegateCommand(_ => executed = true, _ => true);

        command.Execute();

        executed.Should().BeTrue();
    }
    
    [Fact]
    public void Execute_ShouldNotCallAction_WhenCanExecuteIsFalse()
    {
        var executed = false;
        var command = new DelegateCommand(_ => executed = true, _ => false);

        command.Execute();

        executed.Should().BeFalse();
    }
    
    [Fact]
    public void CanExecuteChanged_ShouldBeRaised_WhenRaiseCanExecuteChangedIsCalled()
    {
        var command = new DelegateCommand(_ => { }, _ => true);
        var raised = false;
        command.CanExecuteChanged += (s, e) => raised = true;

        command.RaiseCanExecuteChanged();

        raised.Should().BeTrue();
    }
    
    [Fact]
    public void Execute_ShouldBeCalled_WhenICommandExecuteIsCalled()
    {
        var executed = false;
        var command = new DelegateCommand(_ => executed = true, _ => true);

        ((ICommand)command).Execute(null);

        executed.Should().BeTrue();
    }
    
    [Fact]
    public void CanExecute_ShouldBeCalled_WhenICommandCanExecuteIsCalled()
    {
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecute = ((ICommand)command).CanExecute(null);

        canExecute.Should().BeTrue();
    }
    
    [Fact]
    public void Execute_ShouldNotThrow_WhenParameterIsNull()
    {
        var command = new DelegateCommand(_ => { }, _ => true);

        Action act = () => command.Execute();

        act.Should().NotThrow();
    }
    
    [Fact]
    public void CanExecute_ShouldNotThrow_WhenParameterIsNull()
    {
        var command = new DelegateCommand(_ => { }, _ => true);

        Action act = () => command.CanExecute();

        act.Should().NotThrow();
    }
    
    [Fact]
    public void Execute_ShouldNotThrow_WhenICommandExecuteIsCalledWithNull()
    {
        var command = new DelegateCommand(_ => { }, _ => true);

        Action act = () => ((ICommand)command).Execute(null);

        act.Should().NotThrow();
    }
    
    [Fact]
    public void CanExecute_ShouldNotThrow_WhenICommandCanExecuteIsCalledWithNull()
    {
        var command = new DelegateCommand(_ => { }, _ => true);

        Action act = () => ((ICommand)command).CanExecute(null);

        act.Should().NotThrow();
    }
}
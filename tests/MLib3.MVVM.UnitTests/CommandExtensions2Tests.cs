using System.ComponentModel;
using MLib3.MVVM.UnitTests.Mocks;

namespace MLib3.MVVM.UnitTests;

public class CommandExtensions2Tests
{
    [Fact]
    public void ShouldRaiseCanExecute_WhenPropertyChanges()
    {
        // Arrange
        var viewModel = new A();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        command.ReactTo(viewModel, x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name);
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(1);
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_WhenPropertyChanges2()
    {
        // Arrange
        var viewModel = new A();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        command.ReactTo(viewModel, x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name);
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(3);
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_WhenPropertyChanges3()
    {
        // Arrange
        var viewModel = new TestVM();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        command.ReactTo(viewModel, x => x.A).ThenTo(x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name);
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(3);
    }
}
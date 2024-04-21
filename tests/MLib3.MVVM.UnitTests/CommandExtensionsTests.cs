using System.ComponentModel;
using MLib3.MVVM.UnitTests.Mocks;

namespace MLib3.MVVM.UnitTests;

public class CommandExtensionsTests
{
    [Fact]
    public void ShouldRaiseCanExecute_WhenLastPropertyInPathChanges()
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
    public void ShouldRaiseCanExecute_WhenSomePropertyInThePathChanges()
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
    public void ShouldRaiseCanExecute_WhenSomePropertyInMultiplePathsChange()
    {
        // Arrange
        var viewModel = new TestVM();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        command
            .ReactTo(viewModel, x => x.A).ThenTo(x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name)
            .ReactTo(x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name);
        viewModel.A = new A();
        viewModel.A.B = new B();
        viewModel.A.B.C = new C();
        viewModel.A.B.C.Name = "Test";
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(7);
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_WhenAnyPropertyInSourceChanges()
    {
        // Arrange
        var viewModel = new TestVM();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        command.ReactToAll(viewModel);
        viewModel.A = new A();
        viewModel.A.B = new B();
        viewModel.A.B.C = new C();
        viewModel.A.B.C.Name = "Test";
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(2);
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_WhenAnyPropertyInSubPathChanges()
    {
        // Arrange
        var viewModel = new TestVM();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        command
            .ReactTo(viewModel, x => x.A).ThenToAll()
            .ReactTo(x => x.B).ThenToAll();
        viewModel.A = new A();
        viewModel.A.B = new B();
        viewModel.A.B.C = new C();
        viewModel.A.B.C.Name = "Test";
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(4);
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_OnlyWhenReactionIsNotDisposed()
    {
        // Arrange
        var viewModel = new TestVM();
        var command = new DelegateCommand(_ => { }, _ => true);
        var canExecuteChangedHitCounter = 0;
        command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
        
        // Act
        var commandReaction = command
            .ReactTo(viewModel, x => x.A).ThenTo(x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name)
            .ReactTo(x => x.B).ThenTo(x => x.C).ThenTo(x => x.Name).GetDisposable();
        viewModel.A = new A();
        viewModel.A.B = new B();
        viewModel.A.B.C = new C();
        viewModel.A.B.C.Name = "Test";
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";
        commandReaction.Dispose();
        viewModel.A = new A();
        viewModel.A.B = new B();
        viewModel.A.B.C = new C();
        viewModel.A.B.C.Name = "Test";
        viewModel.B = new B();
        viewModel.B.C = new C();
        viewModel.B.C.Name = "Test";

        // Assert
        canExecuteChangedHitCounter.Should().Be(7);
    }
}
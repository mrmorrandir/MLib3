using MLib3.MVVM.UnitTests.Mocks;

namespace MLib3.MVVM.UnitTests;

public class CommandExtensionsTest
{
    [Fact]
    public void ShouldRaiseCanExecute_WhenPropertyChanges()
    {
        // Arrange
        var viewModel = new MockVM();
        var command = new DelegateCommand(_ => {}, _ => true);
        var canExecuteChanged = false;
        var newName = string.Empty;
        command.CanExecuteChanged += (s, e) => canExecuteChanged = true;
    
        // Act
        command.ReactTo(viewModel).Property(x => x.Name).Subscribe(s => newName = s);
        viewModel.Name = "Test";

        // Assert
        canExecuteChanged.Should().BeTrue();
        newName.Should().Be("Test");
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_WhenAnyPropertyChanges()
    {
        // Arrange
        var viewModel = new MockVM();
        var command = new DelegateCommand(_ => {}, _ => true);
        var canExecuteChanged = false;
        command.CanExecuteChanged += (s, e) => canExecuteChanged = true;
    
        // Act
        command.ReactTo(viewModel).AllProperties().Subscribe();
        viewModel.Name = "Test";

        // Assert
        canExecuteChanged.Should().BeTrue();
    }
}
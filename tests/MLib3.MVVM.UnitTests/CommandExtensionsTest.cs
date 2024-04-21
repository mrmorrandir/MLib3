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
        command.CanExecuteChanged += (s, e) => canExecuteChanged = true;
    
        // Act
        CommandExtensions.React(command, viewModel).To(x => x.Name).Start();
        viewModel.Name = "Test";

        // Assert
        canExecuteChanged.Should().BeTrue();
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
        CommandExtensions.React(command, viewModel).ToAll().Start();
        viewModel.Name = "Test";

        // Assert
        canExecuteChanged.Should().BeTrue();
    }
    
    [Fact]
    public void ShouldRaiseCanExecute_WhenChildPropertyChanges()
    {
        // Arrange
        var viewModel = new MockVM();
        var command = new DelegateCommand(_ => {}, _ => true);
        var canExecuteChanged = false;
        command.CanExecuteChanged += (s, e) => canExecuteChanged = true;
    
        // Act
        viewModel.Child = new SubMockVM();
        CommandExtensions.React(command, viewModel).To(x => x.Child).ThenTo(x => x.SomeThing).Start();
        viewModel.Child.SomeThing = "Test";

        // Assert
        canExecuteChanged.Should().BeTrue();
    }
}
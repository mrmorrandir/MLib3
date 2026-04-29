using System.Collections;
using AwesomeAssertions;
using Xunit;

namespace MLib3.MVVM.UnitTests;

public class ValidatableViewModelTests
{
    private sealed class TestValidatableViewModel : ValidatableViewModel { }

    [Fact]
    public void HasErrors_Should_BeFalse_WhenNoErrorsAreSet()
    {
        // Arrange
        var vm = new TestValidatableViewModel();

        // Act & Assert
        vm.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void SetErrors_Should_AddErrors_WhenErrorsAreProvided()
    {
        // Arrange
        var vm = new TestValidatableViewModel();

        // Act
        vm.SetErrors("Name", "Name is required.");

        // Assert
        vm.HasErrors.Should().BeTrue();
        ((IEnumerable<string>)vm.GetErrors("Name")).Should().ContainSingle().Which.Should().Be("Name is required.");
    }

    [Fact]
    public void SetErrors_Should_RaiseErrorsChanged_WhenErrorsChange()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        var raised = new List<string?>();
        vm.ErrorsChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.SetErrors("Name", "Name is required.");

        // Assert
        raised.Should().ContainSingle().Which.Should().Be("Name");
    }

    [Fact]
    public void SetErrors_Should_RaisePropertyChanged_ForHasErrors_WhenErrorStateChanges()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.SetErrors("Name", "Name is required.");

        // Assert
        raised.Should().Contain(nameof(vm.HasErrors));
    }

    [Fact]
    public void SetErrors_Should_NotRaiseErrorsChanged_WhenSameErrorsAreSet()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");
        var raised = new List<string?>();
        vm.ErrorsChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.SetErrors("Name", "Name is required.");

        // Assert
        raised.Should().BeEmpty();
    }

    [Fact]
    public void SetErrors_Should_IgnoreNullOrWhitespaceEntries()
    {
        // Arrange
        var vm = new TestValidatableViewModel();

        // Act
        vm.SetErrors("Name", "  ", null!, "");

        // Assert
        vm.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void SetErrors_Should_DeduplicateErrors()
    {
        // Arrange
        var vm = new TestValidatableViewModel();

        // Act
        vm.SetErrors("Name", "Required", "Required");

        // Assert
        ((IEnumerable<string>)vm.GetErrors("Name")).Should().ContainSingle();
    }

    [Fact]
    public void SetErrors_Should_ClearErrors_WhenEmptyArrayIsProvided()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");

        // Act
        vm.SetErrors("Name");

        // Assert
        vm.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void ClearErrors_WithPropertyName_Should_RemoveErrorsForProperty()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");
        vm.SetErrors("Age", "Age is required.");

        // Act
        vm.ClearErrors("Name");

        // Assert
        vm.HasErrors.Should().BeTrue();
        ((IEnumerable<string>)vm.GetErrors("Name")).Should().BeEmpty();
    }

    [Fact]
    public void ClearErrors_Should_RemoveAllErrors()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");
        vm.SetErrors("Age", "Age is required.");

        // Act
        vm.ClearErrors();

        // Assert
        vm.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void ClearErrors_Should_RaiseErrorsChanged_WithNullPropertyName()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");
        var raised = new List<string?>();
        vm.ErrorsChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.ClearErrors();

        // Assert
        raised.Should().ContainSingle().Which.Should().BeNull();
    }

    [Fact]
    public void ClearErrors_Should_RaisePropertyChanged_ForHasErrors()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.ClearErrors();

        // Assert
        raised.Should().Contain(nameof(vm.HasErrors));
    }

    [Fact]
    public void GetErrors_WithNullPropertyName_Should_ReturnAllErrors()
    {
        // Arrange
        var vm = new TestValidatableViewModel();
        vm.SetErrors("Name", "Name is required.");
        vm.SetErrors("Age", "Age is required.");

        // Act
        var errors = ((IEnumerable<string>)vm.GetErrors(null)).ToList();

        // Assert
        errors.Should().HaveCount(2);
    }

    [Fact]
    public void GetErrors_WithUnknownPropertyName_Should_ReturnEmpty()
    {
        // Arrange
        var vm = new TestValidatableViewModel();

        // Act
        var errors = vm.GetErrors("Unknown").Cast<string>();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void ViewModel_Should_ImplementIValidatableViewModel()
    {
        // Arrange
        var vm = new TestValidatableViewModel();

        // Act & Assert
        vm.Should().BeAssignableTo<IValidatableViewModel>();
    }
}



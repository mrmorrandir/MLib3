using System.Collections;
using AwesomeAssertions;
using Xunit;

namespace MLib3.MVVM.UnitTests;

public class ValidatableViewModelTTests
{
    private sealed class TestModel { }

    private sealed class TestValidatableViewModelT : ValidatableViewModel<TestModel>
    {
        public TestValidatableViewModelT(TestModel model) : base(model) { }
    }

    [Fact]
    public void Constructor_Should_SetModel_WhenModelIsProvided()
    {
        // Arrange
        var model = new TestModel();

        // Act
        var vm = new TestValidatableViewModelT(model);

        // Assert
        vm.Model.Should().BeSameAs(model);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_WhenModelIsNull()
    {
        // Arrange & Act
        var act = () => new TestValidatableViewModelT(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HasErrors_Should_BeFalse_WhenNoErrorsAreSet()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());

        // Act & Assert
        vm.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void SetErrors_Should_AddErrors_WhenErrorsAreProvided()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());

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
        var vm = new TestValidatableViewModelT(new TestModel());
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
        var vm = new TestValidatableViewModelT(new TestModel());
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
        var vm = new TestValidatableViewModelT(new TestModel());
        vm.SetErrors("Name", "Name is required.");
        var raised = new List<string?>();
        vm.ErrorsChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.SetErrors("Name", "Name is required.");

        // Assert
        raised.Should().BeEmpty();
    }

    [Fact]
    public void SetErrors_Should_ClearErrors_WhenEmptyArrayIsProvided()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());
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
        var vm = new TestValidatableViewModelT(new TestModel());
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
        var vm = new TestValidatableViewModelT(new TestModel());
        vm.SetErrors("Name", "Name is required.");
        vm.SetErrors("Age", "Age is required.");

        // Act
        vm.ClearErrors();

        // Assert
        vm.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void GetErrors_WithNullPropertyName_Should_ReturnAllErrors()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());
        vm.SetErrors("Name", "Name is required.");
        vm.SetErrors("Age", "Age is required.");

        // Act
        var errors = ((IEnumerable<string>)vm.GetErrors(null)).ToList();

        // Assert
        errors.Should().HaveCount(2);
    }

    [Fact]
    public void Model_Should_BeUpdatable_WhenSetterIsCalled()
    {
        // Arrange
        var original = new TestModel();
        var updated = new TestModel();
        var vm = new TestValidatableViewModelT(original);

        // Act
        vm.Model = updated;

        // Assert
        vm.Model.Should().BeSameAs(updated);
    }

    [Fact]
    public void ViewModel_Should_ImplementIValidatableViewModel()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IValidatableViewModel>();
    }

    [Fact]
    public void ViewModel_Should_ImplementIViewModelT()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IViewModel<TestModel>>();
    }

    [Fact]
    public void ViewModel_Should_ImplementIValidatableViewModelT()
    {
        // Arrange
        var vm = new TestValidatableViewModelT(new TestModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IValidatableViewModel<TestModel>>();
    }
}


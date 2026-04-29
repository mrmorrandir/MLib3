using AwesomeAssertions;
using Xunit;

namespace MLib3.MVVM.UnitTests;

internal sealed class TestViewModelValidator : ViewModelValidator
{
    public void RaisePropertyChanged(string propertyName) => OnPropertyChanged(propertyName);
}

internal sealed class TestViewModelValidatorTModel { }

internal sealed class TestViewModelValidatorT : ViewModelValidator<TestViewModelValidatorTModel>
{
    public TestViewModelValidatorT(TestViewModelValidatorTModel model) : base(model) { }
}

public class ViewModelValidatorTests
{
    [Fact]
    public void ViewModel_Should_ImplementIViewModel()
    {
        // Arrange
        var vm = new TestViewModelValidator();

        // Act & Assert
        vm.Should().BeAssignableTo<IViewModel>();
    }

    [Fact]
    public void ViewModel_Should_RaisePropertyChanged_WhenPropertyChanges()
    {
        // Arrange
        var vm = new TestViewModelValidator();
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.RaisePropertyChanged(nameof(IViewModel));

        // Assert
        raised.Should().ContainSingle();
    }
}

public class ViewModelValidatorTTests
{
    [Fact]
    public void Constructor_Should_SetModel_WhenModelIsProvided()
    {
        // Arrange
        var model = new TestViewModelValidatorTModel();

        // Act
        var vm = new TestViewModelValidatorT(model);

        // Assert
        vm.Model.Should().BeSameAs(model);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_WhenModelIsNull()
    {
        // Arrange & Act
        var act = () => new TestViewModelValidatorT(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ViewModel_Should_ImplementIViewModelT()
    {
        // Arrange
        var vm = new TestViewModelValidatorT(new TestViewModelValidatorTModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IViewModel<TestViewModelValidatorTModel>>();
    }

}




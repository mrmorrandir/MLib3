using AwesomeAssertions;
using Xunit;

namespace MLib3.MVVM.UnitTests;

public class ViewModelTests
{
    private sealed class TestViewModel : ViewModel
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }

    [Fact]
    public void ViewModel_Should_ImplementIViewModel()
    {
        // Arrange
        var vm = new TestViewModel();

        // Act & Assert
        vm.Should().BeAssignableTo<IViewModel>();
    }

    [Fact]
    public void ViewModel_Should_RaisePropertyChanged_WhenPropertyChanges()
    {
        // Arrange
        var vm = new TestViewModel();
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.Name = "test";

        // Assert
        raised.Should().Contain(nameof(TestViewModel.Name));
    }

    [Fact]
    public void ViewModel_Should_RaisePropertyChanging_WhenPropertyChanges()
    {
        // Arrange
        var vm = new TestViewModel();
        var raised = new List<string?>();
        vm.PropertyChanging += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.Name = "test";

        // Assert
        raised.Should().Contain(nameof(TestViewModel.Name));
    }

    [Fact]
    public void ViewModel_Should_NotRaisePropertyChanged_WhenValueIsUnchanged()
    {
        // Arrange
        var vm = new TestViewModel();
        vm.Name = "same";
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.Name = "same";

        // Assert
        raised.Should().BeEmpty();
    }
}


using AwesomeAssertions;
using Xunit;

namespace MLib3.MVVM.UnitTests;

public class ViewModelTrackedTests
{
    private sealed class TestModel { }

    private sealed class TestViewModelTracked : ViewModelTracked<TestModel>
    {
        private string _name = string.Empty;

        public TestViewModelTracked(TestModel model) : base(model) { }

        public string Name
        {
            get => _name;
            set => SetTrackedProperty(_name, value, v => _name = v);
        }

        public bool PublicSetTrackedProperty<TProp>(TProp oldValue, TProp newValue, System.Action<TProp> setter, string? propertyName = "TestProp")
            => SetTrackedProperty(oldValue, newValue, setter, propertyName);
    }

    [Fact]
    public void Constructor_Should_SetModel_WhenModelIsProvided()
    {
        // Arrange
        var model = new TestModel();

        // Act
        var vm = new TestViewModelTracked(model);

        // Assert
        vm.Model.Should().BeSameAs(model);
    }

    [Fact]
    public void HasChanges_Should_BeFalse_Initially()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.HasChanges.Should().BeFalse();
    }

    [Fact]
    public void IsDeleted_Should_BeFalse_Initially()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void IsEditable_Should_BeFalse_Initially()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.IsEditable.Should().BeFalse();
    }

    [Fact]
    public void IsNew_Should_BeFalse_Initially()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.IsNew.Should().BeFalse();
    }

    [Fact]
    public void IsSelected_Should_BeFalse_Initially()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.IsSelected.Should().BeFalse();
    }

    [Fact]
    public void SetTrackedProperty_Should_ReturnTrue_WhenValueChanges()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var backingField = string.Empty;

        // Act
        var result = vm.PublicSetTrackedProperty(backingField, "new", v => backingField = v);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void SetTrackedProperty_Should_ReturnFalse_WhenValueIsUnchanged()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var backingField = "same";

        // Act
        var result = vm.PublicSetTrackedProperty(backingField, "same", v => backingField = v);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SetTrackedProperty_Should_SetHasChanges_WhenValueChanges()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act
        vm.Name = "changed";

        // Assert
        vm.HasChanges.Should().BeTrue();
    }

    [Fact]
    public void SetTrackedProperty_Should_NotSetHasChanges_WhenValueIsUnchanged()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act
        vm.Name = string.Empty;

        // Assert
        vm.HasChanges.Should().BeFalse();
    }

    [Fact]
    public void SetTrackedProperty_Should_RaisePropertyChanged_WhenValueChanges()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.Name = "changed";

        // Assert
        raised.Should().Contain(nameof(TestViewModelTracked.Name));
    }

    [Fact]
    public void SetTrackedProperty_Should_RaisePropertyChanging_WhenValueChanges()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanging += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.Name = "changed";

        // Assert
        raised.Should().Contain(nameof(TestViewModelTracked.Name));
    }

    [Fact]
    public void SetTrackedProperty_Should_NotRaisePropertyChanged_WhenValueIsUnchanged()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        vm.Name = "same";
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.Name = "same";

        // Assert
        raised.Should().NotContain(nameof(TestViewModelTracked.Name));
    }

    [Fact]
    public void HasChanges_Should_RaisePropertyChanged_WhenSet()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.HasChanges = true;

        // Assert
        raised.Should().Contain(nameof(vm.HasChanges));
    }

    [Fact]
    public void IsDeleted_Should_RaisePropertyChanged_WhenSet()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.IsDeleted = true;

        // Assert
        raised.Should().Contain(nameof(vm.IsDeleted));
    }

    [Fact]
    public void IsEditable_Should_RaisePropertyChanged_WhenSet()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.IsEditable = true;

        // Assert
        raised.Should().Contain(nameof(vm.IsEditable));
    }

    [Fact]
    public void IsNew_Should_RaisePropertyChanged_WhenSet()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.IsNew = true;

        // Assert
        raised.Should().Contain(nameof(vm.IsNew));
    }

    [Fact]
    public void IsSelected_Should_RaisePropertyChanged_WhenSet()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());
        var raised = new List<string?>();
        vm.PropertyChanged += (_, args) => raised.Add(args.PropertyName);

        // Act
        vm.IsSelected = true;

        // Assert
        raised.Should().Contain(nameof(vm.IsSelected));
    }

    [Fact]
    public void ViewModel_Should_ImplementIViewModelTrackedT()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IViewModelTracked<TestModel>>();
    }

    [Fact]
    public void ViewModel_Should_ImplementIValidatableViewModelT()
    {
        // Arrange
        var vm = new TestViewModelTracked(new TestModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IValidatableViewModel<TestModel>>();
    }
}


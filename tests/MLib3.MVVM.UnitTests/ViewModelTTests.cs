using AwesomeAssertions;
using Xunit;

namespace MLib3.MVVM.UnitTests;

public class ViewModelTTests
{
    private sealed class TestModel { }

    private sealed class TestViewModelT : ViewModel<TestModel>
    {
        public TestViewModelT(TestModel model) : base(model) { }
    }

    [Fact]
    public void Constructor_Should_SetModel_WhenModelIsProvided()
    {
        // Arrange
        var model = new TestModel();

        // Act
        var vm = new TestViewModelT(model);

        // Assert
        vm.Model.Should().BeSameAs(model);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_WhenModelIsNull()
    {
        // Arrange & Act
        var act = () => new TestViewModelT(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ViewModel_Should_ImplementIViewModelT()
    {
        // Arrange
        var vm = new TestViewModelT(new TestModel());

        // Act & Assert
        vm.Should().BeAssignableTo<IViewModel<TestModel>>();
    }
}


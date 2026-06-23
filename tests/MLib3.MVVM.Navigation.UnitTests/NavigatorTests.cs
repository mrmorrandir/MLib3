using FluentAssertions;
using MLib3.MVVM.Navigation;
using NSubstitute;
using Xunit;

namespace MLib3.MVVM.Navigation.UnitTests;

public class NavigatorTests
{
    private readonly INavigator<IViewModel> _genericNavigator = Substitute.For<INavigator<IViewModel>>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly Navigator _sut;

    public NavigatorTests()
    {
        _sut = new Navigator(_serviceProvider, _genericNavigator);
    }

    [Fact]
    public void Push_ShouldDelegateToGenericNavigator()
    {
        // Arrange
        var vm = Substitute.For<IViewModel>();

        // Act
        _sut.Push(vm);

        // Assert
        _genericNavigator.Received(1).Push(vm);
    }

    [Fact]
    public void Pop_ShouldDelegateToGenericNavigator()
    {
        // Act
        _sut.Pop();

        // Assert
        _genericNavigator.Received(1).Pop();
    }

    [Fact]
    public void Goto_ShouldDelegateToGenericNavigator()
    {
        // Arrange
        var vm = Substitute.For<IViewModel>();

        // Act
        _sut.Goto(vm);

        // Assert
        _genericNavigator.Received(1).Goto(vm);
    }

    [Fact]
    public void Slide_ShouldDelegateToGenericNavigator()
    {
        // Arrange
        var vm = Substitute.For<IViewModel>();

        // Act
        _sut.Slide(vm);

        // Assert
        _genericNavigator.Received(1).Slide(vm);
    }

    [Fact]
    public void PushGeneric_ShouldResolveFromDIAndPush()
    {
        // Arrange
        var vm = Substitute.For<ITestViewModel>();
        _serviceProvider.GetService(typeof(ITestViewModel)).Returns(vm);

        // Act
        var result = _sut.Push<ITestViewModel>();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(vm);
        _genericNavigator.Received(1).Push(vm);
    }

    [Fact]
    public void PushGeneric_ShouldReturnErrorIfNotFoundInDI()
    {
        // Arrange
        _serviceProvider.GetService(typeof(ITestViewModel)).Returns(null);

        // Act
        var result = _sut.Push<ITestViewModel>();

        // Assert
        result.IsFailed.Should().BeTrue();
        _genericNavigator.DidNotReceive().Push(Arg.Any<IViewModel>());
    }

    [Fact]
    public void GotoGeneric_ShouldResolveFromDIAndGoto()
    {
        // Arrange
        var vm = Substitute.For<ITestViewModel>();
        _serviceProvider.GetService(typeof(ITestViewModel)).Returns(vm);

        // Act
        var result = _sut.Goto<ITestViewModel>();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(vm);
        _genericNavigator.Received(1).Goto(vm);
    }

    [Fact]
    public void SlideGeneric_ShouldResolveFromDIAndSlide()
    {
        // Arrange
        var vm = Substitute.For<ITestViewModel>();
        _serviceProvider.GetService(typeof(ITestViewModel)).Returns(vm);

        // Act
        var result = _sut.Slide<ITestViewModel>();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(vm);
        _genericNavigator.Received(1).Slide(vm);
    }

    public interface ITestViewModel : IViewModel { }
}

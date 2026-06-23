using FluentAssertions;
using Microsoft.Extensions.Logging;
using MLib3.MVVM.Navigation;
using NSubstitute;
using Xunit;

namespace MLib3.MVVM.Navigation.UnitTests;

public class NavigatorGenericTests
{
    private readonly ILogger<Navigator<IViewModel>> _logger = Substitute.For<ILogger<Navigator<IViewModel>>>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly Navigator<IViewModel> _sut;

    public NavigatorGenericTests()
    {
        _sut = new Navigator<IViewModel>(_logger, _serviceProvider);
    }

    [Fact]
    public void CurrentViewModel_ShouldThrow_WhenStackIsEmpty()
    {
        // Act
        Action act = () => { var _ = _sut.CurrentViewModel; };

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Push_ShouldUpdateCurrentViewModel()
    {
        // Arrange
        var vm1 = Substitute.For<IViewModel>();
        var vm2 = Substitute.For<IViewModel>();

        // Act
        _sut.Push(vm1);
        var current1 = _sut.CurrentViewModel;
        _sut.Push(vm2);
        var current2 = _sut.CurrentViewModel;

        // Assert
        current1.Should().Be(vm1);
        current2.Should().Be(vm2);
    }

    [Fact]
    public void Push_ShouldNotifyPropertyChanged()
    {
        // Arrange
        var vm = Substitute.For<IViewModel>();
        var propertyName = string.Empty;
        _sut.PropertyChanged += (sender, args) => propertyName = args.PropertyName;

        // Act
        _sut.Push(vm);

        // Assert
        propertyName.Should().Be(nameof(_sut.CurrentViewModel));
    }

    [Fact]
    public void Pop_ShouldReturnToPreviousViewModel()
    {
        // Arrange
        var vm1 = Substitute.For<IViewModel>();
        var vm2 = Substitute.For<IViewModel>();
        _sut.Push(vm1);
        _sut.Push(vm2);

        // Act
        _sut.Pop();

        // Assert
        _sut.CurrentViewModel.Should().Be(vm1);
    }

    [Fact]
    public void Pop_ShouldNotPopLastViewModel()
    {
        // Arrange
        var vm = Substitute.For<IViewModel>();
        _sut.Push(vm);

        // Act
        _sut.Pop();

        // Assert
        _sut.CurrentViewModel.Should().Be(vm);
    }

    [Fact]
    public void Goto_ShouldClearStackAndSetNewViewModel()
    {
        // Arrange
        var vm1 = Substitute.For<IViewModel>();
        var vm2 = Substitute.For<IViewModel>();
        var vm3 = Substitute.For<IViewModel>();
        _sut.Push(vm1);
        _sut.Push(vm2);

        // Act
        _sut.Goto(vm3);

        // Assert
        _sut.CurrentViewModel.Should().Be(vm3);
        
        // Ensure stack was cleared by trying to Pop
        _sut.Pop();
        _sut.CurrentViewModel.Should().Be(vm3);
    }

    [Fact]
    public void Slide_ShouldReplaceCurrentViewModel()
    {
        // Arrange
        var vm1 = Substitute.For<IViewModel>();
        var vm2 = Substitute.For<IViewModel>();
        var vm3 = Substitute.For<IViewModel>();
        _sut.Push(vm1);
        _sut.Push(vm2);

        // Act
        _sut.Slide(vm3);

        // Assert
        _sut.CurrentViewModel.Should().Be(vm3);
        
        // Pop should go back to vm1, because vm2 was replaced by vm3
        _sut.Pop();
        _sut.CurrentViewModel.Should().Be(vm1);
    }

    [Fact]
    public void NavigationChanged_ShouldEmitValues()
    {
        // Arrange
        var vm1 = Substitute.For<IViewModel>();
        var vm2 = Substitute.For<IViewModel>();
        NavigationChangedInfo<IViewModel>? receivedInfo = null;
        _sut.NavigationChanged.Subscribe(info => receivedInfo = info);

        // Act
        _sut.Push(vm1);

        // Assert
        receivedInfo.Should().NotBeNull();
        receivedInfo!.Before.Should().BeNull();
        receivedInfo!.Now.Should().Be(vm1);

        // Act 2
        _sut.Push(vm2);

        // Assert 2
        receivedInfo!.Before.Should().Be(vm1);
        receivedInfo!.Now.Should().Be(vm2);
    }

    [Fact]
    public void ReachedAndLeft_ShouldEmitValues()
    {
        // Arrange
        var vm1 = Substitute.For<IViewModel>();
        var vm2 = Substitute.For<IViewModel>();
        IViewModel? reached = null;
        IViewModel? left = null;
        _sut.Reached.Subscribe(vm => reached = vm);
        _sut.Left.Subscribe(vm => left = vm);

        // Act
        _sut.Push(vm1);

        // Assert
        reached.Should().Be(vm1);
        left.Should().BeNull();

        // Act 2
        _sut.Push(vm2);

        // Assert 2
        reached.Should().Be(vm2);
        left.Should().Be(vm1);
    }
}

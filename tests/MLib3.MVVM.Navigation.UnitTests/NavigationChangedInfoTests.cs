using FluentAssertions;
using MLib3.MVVM.Navigation;
using NSubstitute;
using Xunit;

namespace MLib3.MVVM.Navigation.UnitTests;

public class NavigationChangedInfoTests
{
    [Fact]
    public void Equals_ShouldCompareBeforeAndNow()
    {
        // Arrange
        var before = Substitute.For<IViewModel>();
        var now = Substitute.For<IViewModel>();
        var equalInfo = new NavigationChangedInfo<IViewModel>(before, now);
        var differentBeforeInfo = new NavigationChangedInfo<IViewModel>(null, now);
        var differentNowInfo = new NavigationChangedInfo<IViewModel>(before, Substitute.For<IViewModel>());

        // Act
        var equalsSameTransition = equalInfo.Equals(new NavigationChangedInfo<IViewModel>(before, now));

        // Assert
        equalsSameTransition.Should().BeTrue();
        equalInfo.Equals(differentBeforeInfo).Should().BeFalse();
        equalInfo.Equals(differentNowInfo).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldMatchForEqualValues()
    {
        // Arrange
        var before = Substitute.For<IViewModel>();
        var now = Substitute.For<IViewModel>();
        var info = new NavigationChangedInfo<IViewModel>(before, now);
        var equalInfo = new NavigationChangedInfo<IViewModel>(before, now);

        // Act
        var hashCode = info.GetHashCode();

        // Assert
        hashCode.Should().Be(equalInfo.GetHashCode());
    }
}

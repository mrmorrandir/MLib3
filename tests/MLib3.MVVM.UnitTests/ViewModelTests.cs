using System.Reactive;
using MLib3.MVVM.UnitTests.Mocks;

namespace MLib3.MVVM.UnitTests;

public class ViewModelTests
{
    [Fact]
    public void Set_ShouldSetField()
    {
        var testVM = new MockVM();

        testVM.Name = "Test2";

        testVM.Name.Should().Be("Test2");
    }

    [Fact]
    public void Set_ShouldCallCallback()
    {
        var testVM = new MockVM();

        testVM.Name = "Test2";

        testVM.CalledBack.Should().BeTrue();
        testVM.OldValue.Should().Be("Test");
        testVM.NewValue.Should().Be("Test2");
    }

    [Fact]
    public void Set_ShouldRaisePropertyChanged()
    {
        var testVM = new MockVM();
        var raised = false;
        testVM.PropertyChanged += (sender, args) =>
        {
            raised = true;
            args.PropertyName.Should().Be("Name");
        };

        testVM.Name = "Test2";

        raised.Should().BeTrue();
    }
}
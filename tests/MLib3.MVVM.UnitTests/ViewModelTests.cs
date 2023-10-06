using System.Reactive;

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

    [Fact]
    public void Set_ShouldNotifyObservers()
    {
        var testVM = new MockVM();
        var raised = false;
        testVM.Subscribe(info =>
        {
            raised = true;
            info.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            info.Source.Should().Be(testVM);
            info.Name.Should().Be("Name");
            info.Value.Should().Be("Test2");
        });

        testVM.Name = "Test2";

        raised.Should().BeTrue();
    }
}
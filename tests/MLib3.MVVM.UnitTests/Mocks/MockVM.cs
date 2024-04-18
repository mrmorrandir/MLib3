namespace MLib3.MVVM.UnitTests.Mocks;

public class MockVM : ViewModel
{
    private string _name = "Test";
    public bool CalledBack { get; set; } = false;
    public string? OldValue { get; set; } = null;
    public string? NewValue { get; set; } = null;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value, (oldValue, newValue) =>
        {
            CalledBack = true;
            OldValue = oldValue;
            NewValue = newValue;
        });
    }
    
    public MockVM() { }
}
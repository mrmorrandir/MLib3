namespace MLib3.MVVM.UnitTests.Mocks;

public class MockVM : ViewModel
{
    private string _name = nameof(MockVM);
    private SubMockVM? _child = null;
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
    
    public SubMockVM? Child
    {
        get => _child;
        set => Set(ref _child, value);
    }
    
    public MockVM() { }
}

public class SubMockVM : ViewModel
{
    private string _someThing = nameof(SubMockVM);
    
    public string SomeThing
    {
        get => _someThing;
        set => Set(ref _someThing, value);
    }
    
}
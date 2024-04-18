namespace MLib3.MVVM.UnitTests.Mocks;

public class MockModelVM : ViewModel<MockModel>
{
    public string Name
    {
        get => Model.Name;
        set => SetModel(value);
    }
    
    public string? Description
    {
        get => Model.Description;
        set => SetModel(value);
    }
    
    public MockModelVM(MockModel model) : base(model) { }
}
namespace MLib3.MVVM.UnitTests.Mocks;

public class TestVM : ViewModel
{
    private A _a = new A();
    private B _b = new B();
    private DelegateCommand _command = new DelegateCommand(_ => { }, _ => true);

    public A A
    {
        get => _a;
        set => Set(ref _a, value);
    }

    public B B
    {
        get => _b;
        set => Set(ref _b, value);
    }
    
    public TestVM()
    {
        
    }
}
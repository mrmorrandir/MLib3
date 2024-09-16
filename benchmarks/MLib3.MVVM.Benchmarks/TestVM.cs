using MLib3.MVVM.SourceGenerators;

namespace MLib3.MVVM.Benchmarks;

public partial class TestVM : ViewModel<Test>
{
    
    public string Name
    {
        get => Model.Name;
        set => SetModel(value);
    }

    public string Name2
    {
        get => Model.Name2;
        set => SetModel2(value);
    }
    
    public string Name3
    {
        get => Model.Name3;
        set => SetModel2(value);
    }

    [ModelProperty]
    public string Name4
    {
        get;
        set;
    }
    
    public TestVM() : this(new Test()){}
    public TestVM(Test model) : base(model) { }
}
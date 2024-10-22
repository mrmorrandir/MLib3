namespace MLib3.MVVM.SourceGenerators.Target;

[ExposeModelProperty(nameof(FancyDTO.Name))]
public partial class FancyVM : ViewModel<FancyDTO>
{
    public FancyVM(FancyDTO model) : base(model)
    {
        Name = "Hallo Welt";
    }
}
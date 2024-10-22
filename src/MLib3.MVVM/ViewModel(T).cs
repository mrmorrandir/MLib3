using System.Runtime.CompilerServices;

namespace MLib3.MVVM;

public abstract class ViewModel<TModel> : ViewModel, IViewModel<TModel> where TModel : class 
{
    public TModel Model { get; }

    protected ViewModel(TModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }
}
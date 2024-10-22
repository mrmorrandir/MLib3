using System.Runtime.CompilerServices;

namespace MLib3.MVVM;

public abstract class ViewModelValidator<TModel> : ViewModelValidator, IViewModel<TModel> where TModel : class
{
    public TModel Model { get; }

    protected ViewModelValidator(TModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }
}
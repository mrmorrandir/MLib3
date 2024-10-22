namespace MLib3.MVVM;

public interface IViewModelValidator<TModel> : IViewModelValidator, IViewModel<TModel> where TModel : class
{
}
namespace MLib3.MVVM;

public interface IViewModel<TModel> : IViewModel where TModel : class
{
    TModel Model { get; }
}
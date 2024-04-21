using System.Windows.Input;

namespace MLib3.MVVM
{
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
    }
}

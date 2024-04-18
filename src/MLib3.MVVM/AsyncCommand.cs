using System.Windows.Input;

namespace MLib3.MVVM
{
    /// <summary>
    /// AsyncCommand is used whenever an async operation with async/await should be used within the <see cref="ICommand.Execute(object)"/> body. A <see cref="DelegateCommand"/> is not sufficient for async operations!
    /// </summary>
    public class AsyncCommand : AsyncCommand<object>
    {
        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null, Action<Exception> errorCallback = null) : base(execute, canExecute, errorCallback)
        {
        }
    }
}

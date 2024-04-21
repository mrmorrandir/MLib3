using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace MLib3.MVVM
{
    public class AsyncCommand<T> : ICommandBase, IAsyncCommand<T>
    {
        private readonly Action<Exception> _errorCallback;
        private readonly Func<T?, bool> _canExecute;
        private readonly Func<T?, Task> _execute;

        public AsyncCommand(Func<T?, Task> execute, Action<Exception> errorCallback)
        {
            _execute = execute;
            _canExecute = _ => true;
            _errorCallback = errorCallback;
        }
        
        public AsyncCommand(Func<T?, Task> execute, Func<T?, bool> canExecute, Action<Exception> errorCallback)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorCallback = errorCallback;            
        }

        public bool CanExecute(T? parameter = default)
        {
            return _canExecute(parameter);
        }

        public async Task ExecuteAsync(T? parameter = default)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    await _execute(parameter).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    _errorCallback?.Invoke(ex);
                }
            }
        }

        public event EventHandler? CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute((T)parameter!);
        }
        
        async void ICommand.Execute(object? parameter)
        {
            if (CanExecute((T)parameter!))
                await ExecuteAsync((T)parameter!).ConfigureAwait(true);
        }

        
    }
}

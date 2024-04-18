using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace MLib3.MVVM
{
    public class AsyncCommand<T> : Command, IAsyncCommand<T>
    {
        private Action<Exception> _errorCallback = null;
        private readonly Func<T, bool> _canExecute;
        private readonly Func<T, Task> _execute;
        private Dictionary<object, List<string>> _dependsOn;

        // TODO: IErrorHandler muss automatisch beim CreateCommandBinding mit verbunden werden => z.B. RestRequestAsyncCommand wird gemappt zu RestRequestAsync(object o), CanRestRequest(object o) und RequestRequestHandleError(Exception ex)
        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, Action<Exception> errorCallback = null)
        {
            _errorCallback = errorCallback;
            _execute = execute;
            _canExecute = canExecute;
            _dependsOn = new Dictionary<object, List<string>>();
        }

        public bool CanExecute(T parameter)
        {
            return (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
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

        #region Explicit ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        async void ICommand.Execute(object parameter)
        {
            if (CanExecute((T)parameter))
                await ExecuteAsync((T)parameter).ConfigureAwait(true);
        }
        #endregion

    }
}

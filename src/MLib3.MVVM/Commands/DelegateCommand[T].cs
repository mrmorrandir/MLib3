using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace MLib3.MVVM
{
    public class DelegateCommand<T> : ICommandBase
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;
        private Dictionary<object, List<string>> _dependsOn;

        public DelegateCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute ?? (_ => true);
            _dependsOn = new Dictionary<object, List<string>>();
        }

        public bool CanExecute(T? parameter = default)
        {
            return (_canExecute?.Invoke(parameter) ?? true);
        }

        public void Execute(T? parameter = default)
        {
            if (CanExecute(parameter))
                _execute(parameter);
        }

        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute((T)parameter!);
        }

        void ICommand.Execute(object? parameter)
        {
            Execute((T)parameter!);
        }

        public event EventHandler? CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

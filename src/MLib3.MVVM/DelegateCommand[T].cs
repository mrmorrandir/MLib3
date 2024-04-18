using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace MLib3.MVVM
{
    public class DelegateCommand<T> : Command, ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;
        private Dictionary<object, List<string>> _dependsOn;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _dependsOn = new Dictionary<object, List<string>>();
        }

        public bool CanExecute(T parameter)
        {
            return (_canExecute?.Invoke(parameter) ?? true);
        }

        public void Execute(T parameter)
        {
            if (CanExecute(parameter))
                _execute(parameter);
        }

        #region Explicit ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
        #endregion

        [Obsolete("Use the fluent ReactsTo API instead!")]
        public DelegateCommand<T> DependsOn<TClass>(TClass target, string property) where TClass : INotifyPropertyChanged
        {
            if (!_dependsOn.ContainsKey(target))
            {
                EventInfo e = target.GetType().GetEvent("PropertyChanged");
                Delegate handler = Delegate.CreateDelegate(e.EventHandlerType, this, "OnPropertyChanged");
                target.GetType().GetEvent("PropertyChanged").AddEventHandler(target, handler);
                _dependsOn.Add(target, new List<string>());
            }
            if (!_dependsOn[target].Contains(property)) _dependsOn[target].Add(property);
            return this;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_dependsOn.ContainsKey(sender) && _dependsOn[sender].Contains(e.PropertyName))
                RaiseCanExecuteChanged();
        }
    }
}

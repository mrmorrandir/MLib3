using System.Windows.Input;

namespace MLib3.MVVM
{
    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action<object?> execute, Func<object?, bool> canExecute) : base(execute, canExecute)
        {
        }
    }
}

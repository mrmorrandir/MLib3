using System.Windows.Input;

namespace MLib3.MVVM;

public interface ICommandBase : ICommand
{
    void RaiseCanExecuteChanged();
}
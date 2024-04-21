using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace MLib3.MVVM;

public interface ICommandBase : ICommand
{
    void RaiseCanExecuteChanged();
}
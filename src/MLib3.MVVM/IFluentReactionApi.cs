using System.Windows.Input;

namespace MLib3.MVVM
{
    /// <summary>
    /// AsyncCommand is used whenever an async operation with async/await should be used within the <see cref="ICommand.Execute(object)"/> body. A <see cref="DelegateCommand"/> is not sufficient for async operations!
    /// </summary>
    /// 
    public interface IFluentReactionApi : ICommandReactionPath, ICommandReactionSubPath { }
}

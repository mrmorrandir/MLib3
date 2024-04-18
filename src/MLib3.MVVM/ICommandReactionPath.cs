using System.ComponentModel;

namespace MLib3.MVVM
{
    public interface ICommandReactionPath
    {
        ICommandReactionPath ReactsTo<TClass>(TClass target, string property) where TClass : INotifyPropertyChanged;
        ICommandReactionSubPath ThenTo(string property);
        ICommandReactionPath ThenToAll();
        void Activate();
        void Deactivate();
    }
}

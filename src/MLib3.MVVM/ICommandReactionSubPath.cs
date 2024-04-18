namespace MLib3.MVVM
{
    public interface ICommandReactionSubPath : ICommandReactionPath
    {
        ICommandReactionSubPath And(string property);
    }
}

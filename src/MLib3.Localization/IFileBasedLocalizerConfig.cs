namespace MLib3.Localization;

public interface IFileBasedLocalizerConfig<T> : ILocalizerConfig<T>
{
    public T AddFile(string filename);
}
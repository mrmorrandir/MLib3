namespace MLib3.Configuration;

public interface ISettingsReader<TSettings> where TSettings : class
{
    public Result<TSettings> Read();
}
namespace MLib3.Configuration;

public interface ISettingsManager
{
    public Result Reset();
    public Result Save();
}
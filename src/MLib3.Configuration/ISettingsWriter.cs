namespace MLib3.Configuration;

public interface ISettingsWriter<in TSettings> where TSettings : class
{
    public Result Write(TSettings settings);
}
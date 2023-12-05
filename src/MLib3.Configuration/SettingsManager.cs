namespace MLib3.Configuration;

public class SettingsManager<TSettings> : ISettingsManager where TSettings : class
{
    private readonly ISettingsReader<TSettings> _settingsReader;
    private readonly ISettingsWriter<TSettings> _settingsWriter;
    
    public TSettings Settings { get; private set; }

    public SettingsManager(ISettingsReader<TSettings> settingsReader, ISettingsWriter<TSettings> settingsWriter)
    {
        _settingsReader = settingsReader;
        _settingsWriter = settingsWriter;
        Reset();
    }

    public Result Reset()
    {
        var settingsResult = _settingsReader.Read();
        if (settingsResult.IsSuccess)
            Settings = settingsResult.Value;
        return settingsResult.ToResult();
    }

    public Result Save()
    { 
        var writeResult = _settingsWriter.Write(Settings);
        return writeResult;
    }
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MLib3.Configuration;

public class SettingsWriter<TSettings> : ISettingsWriter<TSettings> where TSettings : class
{
    private readonly string _filename;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public SettingsWriter(string filename)
    {
        _filename = filename;
    }
    
    public Result Write(TSettings settings)
    {
        return Result.Try(() =>
        {
            var json = JsonSerializer.Serialize(settings, _jsonSerializerOptions);
            System.IO.File.WriteAllText(_filename, json);
        },ex => new ExceptionalError($"Failed to save settings to file {_filename}", ex));
    }
}
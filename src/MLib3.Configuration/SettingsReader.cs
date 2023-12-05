using System.Text.Json;
using System.Text.Json.Serialization;

namespace MLib3.Configuration;

public class SettingsReader<TSettings> : ISettingsReader<TSettings> where TSettings : class
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

    public SettingsReader(string filename)
    {
        _filename = filename;
    }
    
    public Result<TSettings> Read()
    {
        var readResult = Result.Try(() => System.IO.File.ReadAllText(_filename));
        if (readResult.IsFailed)
            return Result.Fail(new Error($"Failed to read settings from file {_filename}.").CausedBy(readResult.Errors));

        var deserializeResult = Result.Try(() => JsonSerializer.Deserialize<TSettings>(readResult.Value, _jsonSerializerOptions));
        if (deserializeResult.IsFailed)
            return Result.Fail(new Error($"Failed to deserialize settings from file {_filename} to type {typeof(TSettings).Name}.").CausedBy(deserializeResult.Errors));
        
        var settings = deserializeResult.Value!;
        return Result.Ok(settings);
    }
}
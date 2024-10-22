using System.Text.Json;
using System.Text.Json.Serialization;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Json;

public class IResultsConverter : JsonConverter<IResults>
{
    public override IResults Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;
            return JsonSerializer.Deserialize<Results>(root.GetRawText(), options);
        }
    }

    public override void Write(Utf8JsonWriter writer, IResults value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (Results)value, options);
    }
}
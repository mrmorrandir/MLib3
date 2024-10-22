using System.Text.Json;
using System.Text.Json.Serialization;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Json;

public class IMetaConverter : JsonConverter<IMeta>
{
    public override IMeta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;
            return JsonSerializer.Deserialize<Meta>(root.GetRawText(), options);
        }
    }

    public override void Write(Utf8JsonWriter writer, IMeta value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (Meta)value, options);
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MLib3.Protocols.Measurements.Abstractions;

namespace MLib3.Protocols.Measurements.Json;

public class IProductConverter : JsonConverter<IProduct>
{
    public override IProduct Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;
            return JsonSerializer.Deserialize<Product>(root.GetRawText(), options);
        }
    }

    public override void Write(Utf8JsonWriter writer, IProduct value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (Product)value, options);
    }
}
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MLib3.Protocols.Measurements.Abstractions;
using MLib3.Protocols.Measurements.Json;

namespace MLib3.Protocols.Measurements.Json;

public class IElementConverter : JsonConverter<IElement>
{
    public override IElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;
            string typeName = root.GetProperty("Type").GetString();
            if (TypeMapping.TypeNameToType.TryGetValue(typeName, out Type type))
            {
                return (IElement)JsonSerializer.Deserialize(root.GetRawText(), type, options);
            }
            throw new JsonException($"Unknown type: {typeName}");
        }
    }

    public override void Write(Utf8JsonWriter writer, IElement value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (TypeMapping.TypeToTypeName.TryGetValue(value.GetType(), out string typeName))
        {
            writer.WriteString("Type", typeName);
        }
        else
        {
            throw new JsonException($"Unknown type: {value.GetType()}");
        }
        var properties = value.GetType().GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0); // Exclude indexer properties
        foreach (var property in properties)
        {
            writer.WritePropertyName(property.Name);
            JsonSerializer.Serialize(writer, property.GetValue(value), options);
        }
        writer.WriteEndObject();
    }
}
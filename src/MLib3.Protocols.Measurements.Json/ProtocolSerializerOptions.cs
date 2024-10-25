using System.Text.Json;
using System.Text.Json.Serialization;
using MLib3.Protocols.Measurements.Json.Converters;

namespace MLib3.Protocols.Measurements.Json;

public static class ProtocolSerializerOptions
{
    public static JsonSerializerOptions Default => new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ElementConverter() }
    };
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MLib3.Protocols.Measurements.Json;

public static class ProtocolSerializerOptions
{
    public static JsonSerializerOptions Default => new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new IElementConverter(), new IMetaConverter(), new IProductConverter(), new IResultsConverter() }
    };
}
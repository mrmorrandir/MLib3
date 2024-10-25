namespace MLib3.Protocols.Measurements.Json;

public static class JsonTypeMapping
{
    public static readonly Dictionary<string, Type> TypeNameToType = new()
    {
        { "Section", typeof(Serialization.Section) },
        { "CommentSetting", typeof(Serialization.CommentSetting) },
        { "Comment", typeof(Serialization.Comment) },
        { "InfoSetting", typeof(Serialization.InfoSetting) },
        { "Info", typeof(Serialization.Info) },
        { "FlagSetting", typeof(Serialization.FlagSetting) },
        { "Flag", typeof(Serialization.Flag) },
        { "ValueSetting", typeof(Serialization.ValueSetting) },
        { "Value", typeof(Serialization.Value) },
        { "RawDataSetting", typeof(Serialization.RawDataSetting) },
        { "RawData", typeof(Serialization.RawData) }
    };

    public static readonly Dictionary<Type, string> TypeToTypeName = new()
    {
        { typeof(Serialization.Section), "Section" },
        { typeof(Serialization.CommentSetting), "CommentSetting" },
        { typeof(Serialization.Comment), "Comment" },
        { typeof(Serialization.InfoSetting), "InfoSetting" },
        { typeof(Serialization.Info), "Info" },
        { typeof(Serialization.FlagSetting), "FlagSetting" },
        { typeof(Serialization.Flag), "Flag" },
        { typeof(Serialization.ValueSetting), "ValueSetting" },
        { typeof(Serialization.Value), "Value" },
        { typeof(Serialization.RawDataSetting), "RawDataSetting" },
        { typeof(Serialization.RawData), "RawData" }
    };
}
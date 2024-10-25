namespace MLib3.Protocols.Measurements.Serialization.Json.Common;

public static class JsonTypeMapping
{
    public static readonly Dictionary<string, Type> TypeNameToType = new()
    {
        { "Section", typeof(Section) },
        { "CommentSetting", typeof(CommentSetting) },
        { "Comment", typeof(Comment) },
        { "InfoSetting", typeof(InfoSetting) },
        { "Info", typeof(Info) },
        { "FlagSetting", typeof(FlagSetting) },
        { "Flag", typeof(Flag) },
        { "ValueSetting", typeof(ValueSetting) },
        { "Value", typeof(Value) },
        { "RawDataSetting", typeof(RawDataSetting) },
        { "RawData", typeof(RawData) }
    };

    public static readonly Dictionary<Type, string> TypeToTypeName = new()
    {
        { typeof(Section), "Section" },
        { typeof(CommentSetting), "CommentSetting" },
        { typeof(Comment), "Comment" },
        { typeof(InfoSetting), "InfoSetting" },
        { typeof(Info), "Info" },
        { typeof(FlagSetting), "FlagSetting" },
        { typeof(Flag), "Flag" },
        { typeof(ValueSetting), "ValueSetting" },
        { typeof(Value), "Value" },
        { typeof(RawDataSetting), "RawDataSetting" },
        { typeof(RawData), "RawData" }
    };
}
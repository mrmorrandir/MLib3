using System;
using System.Collections.Generic;
using MLib3.Protocols.Measurements;

namespace MLib3.Protocols.Measurements.Json;

public static class TypeMapping
{
    public static readonly Dictionary<string, Type> TypeNameToType = new Dictionary<string, Type>
    {
        { "Section", typeof(Section) },
        { "CommentSetting", typeof(CommentSetting) },
        { "Comment", typeof(Comment) },
        { "InfoSetting", typeof(InfoSetting) },
        { "Info", typeof(Info) },
        { "FlagSetting", typeof(FlagSetting) },
        { "Flag", typeof(Flag) },
        { "ValueSetting", typeof(ValueSetting) },
        { "Value", typeof(Value) }
    };

    public static readonly Dictionary<Type, string> TypeToTypeName = new Dictionary<Type, string>
    {
        { typeof(Section), "Section" },
        { typeof(CommentSetting), "CommentSetting" },
        { typeof(Comment), "Comment" },
        { typeof(InfoSetting), "InfoSetting" },
        { typeof(Info), "Info" },
        { typeof(FlagSetting), "FlagSetting" },
        { typeof(Flag), "Flag" },
        { typeof(ValueSetting), "ValueSetting" },
        { typeof(Value), "Value" }
    };
}
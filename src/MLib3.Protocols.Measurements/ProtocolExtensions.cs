namespace MLib3.Protocols.Measurements;

public static class ProtocolExtensions
{
    public static IResults Add(this Protocol protocol, params Element[] elements) => protocol.Results.Add(elements);
    
    public static IResults Add(this Protocol protocol, Results additionalResults) => protocol.Results.Add(additionalResults);
    
    public static IResults AddRange(this Protocol protocol, IEnumerable<Element> elements) => protocol.Results.AddRange(elements);

    public static IResults AddComment(this Protocol protocol, string name, string? description = null, string? text = null, Extensions? extensions = null) => protocol.Results.AddComment(name, description, text, extensions);
    
    public static IResults AddFlag(this Protocol protocol, string name, string? description = null, bool? ok = null, Extensions? extensions = null) => protocol.Results.AddFlag(name, description, ok, extensions);
    
    public static IResults AddInfo(this Protocol protocol, string name, string? description = null, double? precision = null, string? unit = null, double? value = null, Extensions? extensions = null) => protocol.Results.AddInfo(name, description, precision, unit, value, extensions);
    
    public static IResults AddRawData(this Protocol protocol, string name, string? description = null, string? format = null, string? raw = null, Extensions? extensions = null) => protocol.Results.AddRawData(name, description, format, raw, extensions);
    
    public static IResults AddValue(this Protocol protocol, string name, string? description = null, string? unit = null, double? precision = null, double? min = null, double? nom = null, double? max = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null, double? result = null, bool? ok = null, Extensions? extensions = null) => protocol.Results.AddValue(name, description, unit, precision, min, nom, max, minLimitType, maxLimitType, result, ok, extensions);
    
    public static IResults AddSection(this Protocol protocol, string name, string? description = null, bool? ok = null, Extensions? extensions = null, params Element[] elements) => protocol.Results.AddSection(name, description, ok, extensions, elements);
    
    public static IResults AddSection(this Protocol protocol, string name, params Element[] elements) => protocol.Results.AddSection(name, null, null, null, elements);
    
    public static IResults Remove(this Protocol protocol, params Element[] elements) => protocol.Results.Remove(elements);
    
    public static IResults Remove(this Protocol protocol, IResults additionalResults) => protocol.Results.Remove(additionalResults);
    
    public static IResults RemoveRange(this Protocol protocol, IEnumerable<Element> elements) => protocol.Results.RemoveRange(elements);
    
    public static IResults Clear(this Protocol protocol) => protocol.Results.Clear();
}
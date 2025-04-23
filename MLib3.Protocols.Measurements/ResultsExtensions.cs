using System.Linq;

namespace MLib3.Protocols.Measurements;

public static class ResultsExtensions
{
    public static IResults Add(this IResults results, params Element[] elements)
    {
        return results.AddRange(elements.AsEnumerable());
    }
    
    public static IResults AddRange(this IResults results, IEnumerable<Element> elements)
    {
        var sourceArray = elements as Element[] ?? elements.ToArray();
        if (sourceArray.Any(x => x is null))
            throw new ArgumentNullException(nameof(elements), "Elements cannot contain null values.");
        foreach(var element in sourceArray)
        {
            if (results.Data.Any(x => x.Name == element.Name))
                throw new ArgumentException($"Element with name '{element.Name}' already exists.");
            if (sourceArray.Count(x => x.Name == element.Name) > 1)
                throw new ArgumentException($"Element with name '{element.Name}' is not unique.");
        }
        results.Data.AddRange(sourceArray);
        return results;
    }
    
    public static IResults AddComment(this IResults results, string name, string? description = null, string? text = null, Extensions? extensions = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        if (results.Data.Any(x => x.Name == name))
            throw new ArgumentException($"Element with name '{name}' already exists.");
        
        var comment = new Comment(name, description, text , extensions);
        results.Data.Add(comment);
        return results;
    }
    
    public static IResults AddFlag(this IResults results, string name, string? description = null, bool? ok = null, Extensions? extensions = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        if (results.Data.Any(x => x.Name == name))
            throw new ArgumentException($"Element with name '{name}' already exists.");
        
        var flag = new Flag(name, description, ok, extensions);
        results.Data.Add(flag);
        return results;
    }

    public static IResults AddInfo(this IResults results, string name, string? description = null, double? precision = null, string? unit = null, double? value = null, Extensions? extensions = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        if (results.Data.Any(x => x.Name == name))
            throw new ArgumentException($"Element with name '{name}' already exists.");
        
        var info = new Info(name, description, unit, precision, value, extensions);
        results.Data.Add(info);
        return results;
    }
    
    public static IResults AddRawData(this IResults results, string name, string? description = null, string? format = null, string? raw = null, Extensions? extensions = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        if (results.Data.Any(x => x.Name == name))
            throw new ArgumentException($"Element with name '{name}' already exists.");
        
        var rawData = new RawData(name, description, format, raw, extensions);
        results.Data.Add(rawData);
        return results;
    }
    
    public static IResults AddValue(this IResults results, string name, string? description = null, string? unit = null, double? precision = null, double? min = null, double? nom = null, double? max = null, ValueLimitType? minLimitType = null, ValueLimitType? maxLimitType = null, double? result = null, bool? ok = null, Extensions? extensions = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        if (results.Data.Any(x => x.Name == name))
            throw new ArgumentException($"Element with name '{name}' already exists.");
        
        var value = new Value(name, description, unit, precision, min, nom, max, minLimitType, maxLimitType, result, ok, extensions);
        results.Data.Add(value);
        return results;
    }
    
    public static IResults AddSection(this IResults results, string name, string? description = null, bool? ok = null, Extensions? extensions = null, params Element[] elements)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        if (results.Data.Any(x => x.Name == name))
            throw new ArgumentException($"Element with name '{name}' already exists.");
        
        var section = new Section(name, description, ok, extensions);
        section.Data.AddRange(elements);
        results.Data.Add(section);
        return results;
    }
}
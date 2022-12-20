namespace MLib3.Protocols.Measurements;

public class Results : IResults
{
    public bool OK { get; set; }
    public IElement this[string name]
    {
        get
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            return Data.Single(e => e.Name == name);
        }
    }
    public IExtensions? Extensions { get; set; }    
    public IEnumerable<IElement> Data { get; set; }
    
    public Results(IEnumerable<IElement>? elements = null, bool ok = false)
    {
        OK = ok;
        Data = elements?.ToList() ?? new List<IElement>();
    }
    
    public IResults Add(IElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));
        if (Data.Any(e => e.Name == element.Name))
            throw new ArgumentException($"Element with name {element.Name} already exists.");
        Data = Data.Append(element);
        return this;
    }

    public IResults AddRange(IEnumerable<IElement> elements)
    {
        if (elements == null)
            throw new ArgumentNullException(nameof(elements));
        Data = Data.Concat(elements);
        return this;
    }

    public IResults AddRange(params IElement[] elements)
    {
        if (elements == null)
            throw new ArgumentNullException(nameof(elements));
        Data = Data.Concat(elements);
        return this;
    }

    public IResults AddRange(IResults results)
    {
        if (results == null)
            throw new ArgumentNullException(nameof(results));
        Data = Data.Concat(results.Data);
        return this;
    }

    public IResults Remove(IElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));
        Data = Data.Where(e => e.Name != element.Name);
        return this;
    }

    public IResults RemoveRange(IEnumerable<IElement> elements)
    {
        if (elements == null)
            throw new ArgumentNullException(nameof(elements));
        Data = Data.Where(e => !elements.Any(el => el.Name == e.Name));
        return this;
    }

    public IResults RemoveRange(params IElement[] elements)
    {
        if (elements == null)
            throw new ArgumentNullException(nameof(elements));
        Data = Data.Where(e => !elements.Any(el => el.Name == e.Name));
        return this;
    }

    public IResults RemoveRange(IResults results)
    {
        if (results == null)
            throw new ArgumentNullException(nameof(results));
        Data = Data.Where(e => !results.Data.Any(el => el.Name == e.Name));
        return this;
    }

    public IResults Clear()
    {
        Data = new List<IElement>();
        return this;
    }
}
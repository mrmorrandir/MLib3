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
            if (Data.All(x => x.Name != name))
                throw new ArgumentException($"No element with name '{name}' found.", nameof(name));
            return Data.Single(e => e.Name == name);
        }
    }

    public IExtensions? Extensions { get; set; }
    public IList<IElement> Data { get; set; }

    public Results(IEnumerable<IElement>? elements = null, bool ok = false)
    {
        OK = ok;
        Data = elements?.ToList() ?? new List<IElement>();
        Extensions = null;
    }

    public IResults Add(IElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));
        if (Data.Any(e => e.Name == element.Name))
            throw new ArgumentException($"Element with name {element.Name} already exists.");
        Data = Data.Append(element).ToList();
        return this;
    }

    public IResults AddRange(IEnumerable<IElement> elements)
    {
        if (elements is null)
            throw new ArgumentNullException(nameof(elements));
        var list = elements.ToList();
        if (list.Any(e => e is null))
            throw new ArgumentException("One or more elements are null.", nameof(elements));
        if (list.Select(e => e.Name).Distinct().Count() != list.Count)
            throw new ArgumentException("Elements must have unique names.", nameof(elements));
        if (list.Any(e => Data.Any(d => d.Name == e.Name)))
            throw new ArgumentException("One or more elements already exist.", nameof(elements));
        Data = Data.Concat(list).ToList();
        return this;
    }

    public IResults AddRange(IResults results)
    {
        if (results is null)
            throw new ArgumentNullException(nameof(results));
        return AddRange(results.Data.ToArray());
    }

    public IResults Remove(IElement element)
    {
        if (element is null)
            throw new ArgumentNullException(nameof(element));
        if (Data.All(e => e.Name != element.Name))
            throw new ArgumentException($"Element with name {element.Name} does not exist.");
        Data = Data.Where(e => e.Name != element.Name).ToList();
        return this;
    }

    public IResults RemoveRange(IEnumerable<IElement> elements)
    {
        if (elements is null)
            throw new ArgumentNullException(nameof(elements));
        var list = elements.ToList();
        if (list.Any(e => e is null))
            throw new ArgumentException("One or more elements are null.", nameof(elements));
        if (list.Any(e => Data.All(d => d.Name != e.Name)))
            throw new ArgumentException("One or more elements do not exist.", nameof(elements));

        Data = Data.Where(e => !list.Contains(e)).ToList();
        return this;
    }

    public IResults RemoveRange(IResults results)
    {
        if (results is null)
            throw new ArgumentNullException(nameof(results));
        return RemoveRange(results.Data.ToArray());
    }

    public IResults Clear()
    {
        Data = new List<IElement>();
        return this;
    }
}
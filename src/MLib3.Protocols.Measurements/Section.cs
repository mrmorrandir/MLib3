namespace MLib3.Protocols.Measurements;

public class Section : Results, ISection
{
    public string Name { get; }
    public string? Hint { get; set; }

    public Section(ISectionSetting sectionSetting, IEnumerable<IElement>? elements = null, bool ok = false)
    {
        if (sectionSetting == null)
            throw new ArgumentNullException(nameof(sectionSetting));
        Name = sectionSetting.Name;
        Hint = sectionSetting.Hint;
        Data = elements ?? new List<IElement>();
        OK = ok;
    }
}
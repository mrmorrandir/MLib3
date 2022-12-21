namespace MLib3.Protocols.Measurements;

public class Section : Results, ISection
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public Section(ISectionSetting sectionSetting, IEnumerable<IElement>? elements = null, bool ok = false)
    {
        if (sectionSetting == null)
            throw new ArgumentNullException(nameof(sectionSetting));
        Name = sectionSetting.Name;
        Description = sectionSetting.Description;
        Data = elements ?? new List<IElement>();
        OK = ok;
    }
}
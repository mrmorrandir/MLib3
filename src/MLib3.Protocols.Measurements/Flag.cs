namespace MLib3.Protocols.Measurements;

public class Flag : IFlag, IElement, IEvaluated
{
    public IExtensions? Extensions { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool OK { get; set; }
    public Flag() { }

    public Flag(IFlagSetting flagSetting, bool ok = false)
    {
        if (flagSetting == null) throw new ArgumentNullException(nameof(flagSetting));
        Name = flagSetting.Name;
        Description = flagSetting.Description;
        OK = ok;
        Extensions = null;
    }
}
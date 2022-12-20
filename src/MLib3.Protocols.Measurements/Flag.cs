namespace MLib3.Protocols.Measurements;

public class Flag : IFlag, IElement, IEvaluated
{
    public Flag(IFlagSetting flagSetting, bool ok = false)
    {
        if (flagSetting == null) throw new ArgumentNullException(nameof(flagSetting));
        Name = flagSetting.Name;
        Hint = flagSetting.Hint;
        OK = ok;
    }

    public IExtensions? Extensions { get; set; }
    public string Name { get; }
    public string? Hint { get; set; }
    public bool OK { get; set; }
}
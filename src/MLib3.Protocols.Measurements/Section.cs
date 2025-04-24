using System.Linq;

namespace MLib3.Protocols.Measurements;

[XmlInclude(typeof(Comment))]
[XmlInclude(typeof(Value))]
[XmlInclude(typeof(Info))]
[XmlInclude(typeof(Flag))]
[XmlInclude(typeof(RawData))]
[XmlInclude(typeof(Section))]
[XmlInclude(typeof(InfoSetting))]
[XmlInclude(typeof(CommentSetting))]
[XmlInclude(typeof(FlagSetting))]
[XmlInclude(typeof(ValueSetting))]
[XmlInclude(typeof(RawDataSetting))]
public class Section : Element, IResults
{
    [XmlAttribute]
    public bool Ok { get; set; } = false;

    [XmlArray("Data")]
    [XmlArrayItem("Comment", typeof(Comment), Namespace = "")]
    [XmlArrayItem("Value", typeof(Value), Namespace = "")]
    [XmlArrayItem("Info", typeof(Info), Namespace = "")]
    [XmlArrayItem("Flag", typeof(Flag), Namespace = "")]
    [XmlArrayItem("RawData", typeof(RawData), Namespace = "")]
    [XmlArrayItem("Section", typeof(Section), Namespace = "")]
    [XmlArrayItem("InfoSetting", typeof(InfoSetting), Namespace = "")]
    [XmlArrayItem("CommentSetting", typeof(CommentSetting), Namespace = "")]
    [XmlArrayItem("FlagSetting", typeof(FlagSetting), Namespace = "")]
    [XmlArrayItem("ValueSetting", typeof(ValueSetting), Namespace = "")]
    [XmlArrayItem("RawDataSetting", typeof(RawDataSetting), Namespace = "")]
    public List<Element> Data { get; set; } = new();
    
    public Element this[string name]
    {
        get => Data.FirstOrDefault(e => e.Name == name) ?? throw new KeyNotFoundException($"Element with name '{name}' not found.");
        set
        {
            var index = Data.FindIndex(e => e.Name == name);
            if (index >= 0)
                Data[index] = value;
            else
                throw new KeyNotFoundException($"Element with name '{name}' not found.");
        }
    }
    
    public Section() {}
    
     public Section(string name, params Element[] elements) : this(name, null, false, null, elements) {}
     
    public Section(string name, string? description = null, bool? ok = null, Extensions? extensions = null, params Element[] elements) : base(name, description, extensions)
    {
        Ok = ok ?? false;
        Data = new List<Element>(elements);
    }

    public bool Evaluate()
    {
        var result = true;
        foreach (var element in Data)
        {
            if (element is IEvaluable evaluable && !evaluable.Evaluate())
               result = false;
        }

        Ok = result;
        return result;
    }

    public override string ToString() => $"Section: {Name}, Ok: {Ok}, Elements: {Data.Count}";
}
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
public class Results : IResults
{
    [XmlAttribute]
    public bool Ok { get; set; }

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
    
    public Extensions? Extensions { get; set; }
    
    // Indexer for Element Name
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
    
    public Results() {}
    public Results(params Element[] elements)
    {
        Data = new List<Element>(elements);
    }
    public Results(bool ok, params Element[] elements)
    {
        Ok = ok;
        Data = new List<Element>(elements);
    }
    
    public Results(bool? ok = null, Extensions? extensions = null, params Element[] elements)
    {
        Ok = ok ?? false;
        Extensions = extensions;
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

    public override string ToString() => $"Results: Ok: {Ok}, Elements: {Data.Count}";
}
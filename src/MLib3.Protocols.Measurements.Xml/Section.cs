using System.Xml.Serialization;

namespace MLib3.Protocols.Measurements.Xml;

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
public class Section : Element
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
}
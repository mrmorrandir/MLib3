using System.Xml.Linq;

namespace MLib3.Protocols.Measurements.Xml.V2;

/// <summary>
/// This class is used to post-process the XML string after serialization.
/// It is used to rename elements and attributes (and delete elements in some cases) to match the Measurements2 schema.
/// </summary>
public class SerializationPostProcessor : ISerializationPostProcessor
{
    public string Process(string xml)
    {
        var xdoc = XDocument.Parse(xml);
        if (xdoc.Root == null) return xml;
        
        xdoc.Root.Element(nameof(Product))!.Name = "ProductMeta";
        xdoc.Root.Element(nameof(Meta))!.Name = "MeasurementMeta";
        var results = xdoc.Root.Element(nameof(Results))!;
        results.Name = "MeasurementData";
        RenameAttribute(results, "Ok", "OK");
        results.Element(nameof(Results.Data))!.Name = "Measurements";
        
        foreach (var element in xdoc.Descendants().ToArray())
        {
            if (element.Name == nameof(Product.Equipment)) element.Name = "Serialnumber";
            if (element.Name == nameof(Product.Material)) element.Name = "Articlecode";
            if (element.Name == nameof(Product.MaterialText)) element.Name = "OrderKey";
            if (element.Name == nameof(Product.Order)) element.Name = "ProductionOrder";
            
            if (element.Name == nameof(Meta.DeviceId)) element.Name = "DeviceID";
            if (element.Name == nameof(Meta.Operator)) element.Name = "Worker";
            if (element.Name == nameof(Meta.Software)) element.Name = "Program";
            if (element.Name == nameof(Meta.SoftwareVersion)) element.Name = "ProgramVersion";

            if (element.Name == nameof(RawDataSetting))
            {
                // RawDataSetting is not used in the Measurements2 schema
                element.Remove();
            }

            if (element.Name == nameof(InfoSetting))
            {
                // InfoSetting is not used in the Measurements2 schema
                element.Remove();
            }

            if (element.Name == nameof(FlagSetting))
            {
                element.Name = "MeasurementFlagSetting";
                RenameAttribute(element, "Description", "Hint");
            }

            if (element.Name == nameof(ValueSetting))
            {
                element.Name = "MeasurementValueSetting";
                RenameAttribute(element, "Description", "Hint");
            }

            if (element.Name == nameof(Comment))
            {
                element.Name = "MeasurementComment";
                RenameAttribute(element, "Description", "Hint");
            }

            if (element.Name == nameof(Info))
            {
                element.Name = "MeasurementInfo";
                RenameAttribute(element, "Description", "Hint");
            }
            if (element.Name == nameof(Flag))
            {
                element.Name = "MeasurementFlag";
                RenameAttribute(element, "Ok", "OK");
                RenameAttribute(element, "Description", "Hint");
            }

            if (element.Name == nameof(Value) && element.HasElements)
            {
                element.Name = "MeasurementValue";
                RenameAttribute(element, "Ok", "OK");
                RenameAttribute(element, "Description", "Hint");
                element.Element(nameof(Value.Min))!.Name = "ValueLimitMinimum";
                element.Element(nameof(Value.Nom))!.Name = "ValueNominal";
                element.Element(nameof(Value.Max))!.Name = "ValueLimitMaximum";
                element.Element(nameof(Value.MinLimitType))!.Name = "ValueLimitMinimumType";
                element.Element(nameof(Value.MaxLimitType))!.Name = "ValueLimitMaximumType";
                element.Element(nameof(Value.Result))!.Name = "Value";
            }

            if (element.Name == nameof(RawData))
            {
                element.Name = "MeasurementRawData";
                RenameAttribute(element, "Description", "Hint");
            }
            if (element.Name == nameof(Section))
            {
                element.Name = "MeasurementSection";
                RenameAttribute(element, "Ok", "OK");
                RenameAttribute(element, "Description", "Hint");
                element.Element(nameof(Results.Data))!.Name = "Measurements";
            }
        }
        
        return xdoc.ToString();
    }
    
    private void RenameAttribute(XElement element, string oldName, string newName)
    {
        var attribute = element.Attribute(oldName);
        if (attribute == null) return;
        attribute.Remove();
        element.Add(new XAttribute(newName, attribute.Value));
    }
}
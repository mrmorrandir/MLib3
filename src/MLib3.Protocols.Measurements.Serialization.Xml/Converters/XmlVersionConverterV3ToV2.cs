using System.Xml.Linq;

namespace MLib3.Protocols.Measurements.Serialization.Xml.Converters;

internal class XmlVersionConverterV3ToV2 : IXmlVersionConverter
{
    public string Convert(string xml)
    {
        var xdoc = XDocument.Parse(xml);
        if (xdoc.Root == null) return xml;
        
        xdoc.Root.Element(nameof(Product))!.Name = "ProductMeta";
        xdoc.Root.Element(nameof(Meta))!.Name = "MeasurementMeta";
        var results = xdoc.Root.Element(nameof(Results))!;
        results.Name = "MeasurementData";
        RenameAttribute(results, "Ok", "OK");
        var resultsData = results.Element(nameof(Results.Data));
        if (resultsData is not null)
            resultsData.Name = "Measurements";
        
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
            
            if (element.Name == nameof(CommentSetting))
            {
                // CommentSetting is not used in the Measurements2 schema
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
                var min = element.Element(nameof(Value.Min));
                var nom = element.Element(nameof(Value.Nom));
                var max = element.Element(nameof(Value.Max));
                var minType = element.Element(nameof(Value.MinLimitType));
                var maxType = element.Element(nameof(Value.MaxLimitType));
                if (min is not null)
                    min.Name = "ValueLimitMinimum";
                if (nom is not null)
                    nom.Name = "ValueNominal";
                if (max is not null)
                    max.Name = "ValueLimitMaximum";
                if (minType is not null)
                    minType.Name = "ValueLimitMinimumType";
                if (maxType is not null)
                    maxType.Name = "ValueLimitMaximumType";
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
                var min = element.Element(nameof(Value.Min));
                var nom = element.Element(nameof(Value.Nom));
                var max = element.Element(nameof(Value.Max));
                var minType = element.Element(nameof(Value.MinLimitType));
                var maxType = element.Element(nameof(Value.MaxLimitType));
                var result = element.Element(nameof(Value.Result));
                if (min is not null)
                    min.Name = "ValueLimitMinimum";
                if (nom is not null)
                    nom.Name = "ValueNominal";
                if (max is not null)
                    max.Name = "ValueLimitMaximum";
                if (minType is not null)
                    minType.Name = "ValueLimitMinimumType";
                if (maxType is not null)
                    maxType.Name = "ValueLimitMaximumType";
                if (result is not null)
                    result.Name = "Value";
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
                var data = element.Element(nameof(Results.Data));
                if (data is not null)
                    data.Name = "Measurements";
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
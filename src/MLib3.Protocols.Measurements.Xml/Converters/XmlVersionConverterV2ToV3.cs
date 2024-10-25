using System.Xml.Linq;

namespace MLib3.Protocols.Measurements.Xml.Converters
{
    internal class XmlVersionConverterV2ToV3 : IXmlVersionConverter
    {
        public string Convert(string xml)
        {
            var xdoc = XDocument.Parse(xml);
            if (xdoc.Root == null) return xml;

            xdoc.Root.Element("ProductMeta")!.Name = nameof(Serialization.Product);
            xdoc.Root.Element("MeasurementMeta")!.Name = nameof(Serialization.Meta);
            var results = xdoc.Root.Element("MeasurementData")!;
            results.Name = nameof(Serialization.Results);
            RenameAttribute(results, "OK", "Ok");
            results.Element("Measurements")!.Name = nameof(Serialization.Results.Data);

            foreach (var element in xdoc.Descendants().ToArray())
            {
                if (element.Name == "Serialnumber") element.Name = nameof(Serialization.Product.Equipment);
                if (element.Name == "Articlecode") element.Name = nameof(Serialization.Product.Material);
                if (element.Name == "OrderKey") element.Name = nameof(Serialization.Product.MaterialText);
                if (element.Name == "ProductionOrder") element.Name = nameof(Serialization.Product.Order);

                if (element.Name == "DeviceID") element.Name = nameof(Serialization.Meta.DeviceId);
                if (element.Name == "Worker") element.Name = nameof(Serialization.Meta.Operator);
                if (element.Name == "Program") element.Name = nameof(Serialization.Meta.Software);
                if (element.Name == "ProgramVersion") element.Name = nameof(Serialization.Meta.SoftwareVersion);

                if (element.Name == "MeasurementFlagSetting")
                {
                    element.Name = nameof(Serialization.FlagSetting);
                    RenameAttribute(element, "Hint", "Description");
                }

                if (element.Name == "MeasurementValueSetting")
                {
                    element.Name = nameof(Serialization.ValueSetting);
                    RenameAttribute(element, "Hint", "Description");
                    var min = element.Element("ValueLimitMinimum");
                    var nom = element.Element("ValueNominal");
                    var max = element.Element("ValueLimitMaximum");
                    var minType = element.Element("ValueLimitMinimumType");
                    var maxType = element.Element("ValueLimitMaximumType");
                    if (min is not null)
                        min.Name = nameof(Serialization.Value.Min);
                    if (nom is not null)
                        nom.Name = nameof(Serialization.Value.Nom);
                    if (max is not null)
                        max.Name = nameof(Serialization.Value.Max);
                    if (minType is not null)
                        minType.Name = nameof(Serialization.Value.MinLimitType);
                    if (maxType is not null)
                        maxType.Name = nameof(Serialization.Value.MaxLimitType);
                }

                if (element.Name == "MeasurementComment")
                {
                    element.Name = nameof(Serialization.Comment);
                    RenameAttribute(element, "Hint", "Description");
                }

                if (element.Name == "MeasurementInfo")
                {
                    element.Name = nameof(Serialization.Info);
                    RenameAttribute(element, "Hint", "Description");
                }
                if (element.Name == "MeasurementFlag")
                {
                    element.Name = nameof(Serialization.Flag);
                    RenameAttribute(element, "OK", "Ok");
                    RenameAttribute(element, "Hint", "Description");
                }

                if (element.Name == "MeasurementValue" && element.HasElements)
                {
                    element.Name = nameof(Serialization.Value);
                    RenameAttribute(element, "OK", "Ok");
                    RenameAttribute(element, "Hint", "Description");
                    var min = element.Element("ValueLimitMinimum");
                    var nom = element.Element("ValueNominal");
                    var max = element.Element("ValueLimitMaximum");
                    var minType = element.Element("ValueLimitMinimumType");
                    var maxType = element.Element("ValueLimitMaximumType");
                    var result = element.Element("Value");
                    if (min is not null)
                        min.Name = nameof(Serialization.Value.Min);
                    if (nom is not null)
                        nom.Name = nameof(Serialization.Value.Nom);
                    if (max is not null)
                        max.Name = nameof(Serialization.Value.Max);
                    if (minType is not null)
                        minType.Name = nameof(Serialization.Value.MinLimitType);
                    if (maxType is not null)
                        maxType.Name = nameof(Serialization.Value.MaxLimitType);
                    if (result is not null)
                        result.Name = nameof(Serialization.Value.Result);
                }

                if (element.Name == "MeasurementRawData")
                {
                    element.Name = nameof(Serialization.RawData);
                    RenameAttribute(element, "Hint", "Description");
                }
                if (element.Name == "MeasurementSection")
                {
                    element.Name = nameof(Serialization.Section);
                    RenameAttribute(element, "OK", "Ok");
                    RenameAttribute(element, "Hint", "Description");
                    element.Element("Measurements")!.Name = nameof(Serialization.Results.Data);
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
}
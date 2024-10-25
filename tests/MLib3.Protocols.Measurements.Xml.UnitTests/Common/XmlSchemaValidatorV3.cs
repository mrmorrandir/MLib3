using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public static class XmlSchemaValidatorV3
{
    public static Result Validate(string xml)
    {
        var schemas = new XmlSchemaSet();
        schemas.Add("", XmlReader.Create(new StringReader(File.ReadAllText("./SchemaV3.xsd"))));
        
        var validationErrors = new List<string>();
        XDocument.Parse(xml).Validate(schemas, (sender, args) => validationErrors.Add(args.Message));
        return validationErrors.Any() ? Result.Fail(validationErrors) : Result.Ok();
    }
}
using FluentResults;

namespace MLib3.Protocols.Measurements.Serialization.Mappers;

public class DeserializationMapper : IDeserializationMapper
{
    public Result<IProtocol> Map(Protocol protocol)
    {
        var xmlProduct = protocol.Product;
        var xmlMeta = protocol.Meta;
        var xmlResults = protocol.Results;

        var product = new Measurements.Product
        {
            Extensions = MapExtensions(xmlProduct.Extensions),
            Material = xmlProduct.Material,
            MaterialText = xmlProduct.MaterialText,
            Order = xmlProduct.Order,
            Equipment = xmlProduct.Equipment
        };

        var meta = new Measurements.Meta
        {
            Extensions = MapExtensions(xmlMeta.Extensions),
            DeviceId = xmlMeta.DeviceId,
            DeviceName = xmlMeta.DeviceName,
            Software = xmlMeta.Software,
            SoftwareVersion = xmlMeta.SoftwareVersion,
            TestRoutine = xmlMeta.TestRoutine,
            TestRoutineVersion = xmlMeta.TestRoutineVersion,
            Timestamp = xmlMeta.Timestamp,
            Type = xmlMeta.Type,
            Operator = xmlMeta.Operator
        };
        var results = new Measurements.Results
        {
            Ok = xmlResults.Ok,
            Extensions = MapExtensions(xmlResults.Extensions),
            Data = xmlResults.Data.Select(MapElement).ToList()
        };

        return new Measurements.Protocol(product, meta, results);
    }

    private static IExtensions? MapExtensions(Extensions? extensions)
    {
        if (extensions is null)
            return null;

        var result = new Measurements.Extensions();
        foreach (var extension in extensions)
            result.Add(new Measurements.Extension(extension.Key, extension.Value));
        return result;
    }

    private static IElement MapElement(Element element)
    {
        // switch-case for different element types
        return element switch
        {
            Section xmlSection => new Measurements.Section
            {
                Extensions = MapExtensions(xmlSection.Extensions),
                Name = xmlSection.Name,
                Description = xmlSection.Description,
                Data = xmlSection.Data.Select(MapElement).ToList(),
                Ok = xmlSection.Ok
            },
            Comment xmlComment => new Measurements.Comment { Extensions = MapExtensions(xmlComment.Extensions), Name = xmlComment.Name, Description = xmlComment.Description, Text = xmlComment.Text },
            Info xmlInfo => new Measurements.Info { Extensions = MapExtensions(xmlInfo.Extensions), Name = xmlInfo.Name, Description = xmlInfo.Description, Value = xmlInfo.Value },
            Flag xmlFlag => new Measurements.Flag
            {
                Extensions = MapExtensions(xmlFlag.Extensions), Name = xmlFlag.Name, Description = xmlFlag.Description, Ok = xmlFlag.Ok
            },
            Value xmlValue => new Measurements.Value
            {
                Extensions = MapExtensions(xmlValue.Extensions),
                Name = xmlValue.Name,
                Description = xmlValue.Description,
                Unit = xmlValue.Unit,
                Precision = xmlValue.Precision,
                Min = xmlValue.Min,
                Nom = xmlValue.Nom,
                Max = xmlValue.Max,
                MinLimitType = xmlValue.MinLimitType,
                MaxLimitType = xmlValue.MaxLimitType,
                Result = xmlValue.Result,
                Ok = xmlValue.Ok
            },
            RawData xmlRawData => new Measurements.RawData
            {
                Extensions = MapExtensions(xmlRawData.Extensions),
                Name = xmlRawData.Name,
                Description = xmlRawData.Description,
                Format = xmlRawData.Format,
                Raw = xmlRawData.Raw
            },
            CommentSetting xmlCommentSetting => new Measurements.CommentSetting { Extensions = MapExtensions(xmlCommentSetting.Extensions), Name = xmlCommentSetting.Name, Description = xmlCommentSetting.Description },
            InfoSetting xmlInfoSetting => new Measurements.InfoSetting
            {
                Extensions = MapExtensions(xmlInfoSetting.Extensions),
                Name = xmlInfoSetting.Name,
                Description = xmlInfoSetting.Description,
                Precision = xmlInfoSetting.Precision,
                Unit = xmlInfoSetting.Unit
            },
            FlagSetting xmlFlagSetting => new Measurements.FlagSetting { Extensions = MapExtensions(xmlFlagSetting.Extensions), Name = xmlFlagSetting.Name, Description = xmlFlagSetting.Description },
            ValueSetting valueSetting => new Measurements.ValueSetting
            {
                Extensions = MapExtensions(valueSetting.Extensions),
                Name = valueSetting.Name,
                Description = valueSetting.Description,
                Precision = valueSetting.Precision,
                Unit = valueSetting.Unit ?? string.Empty,
                Min = valueSetting.Min,
                Nom = valueSetting.Nom,
                Max = valueSetting.Max,
                MinLimitType = valueSetting.MinLimitType,
                MaxLimitType = valueSetting.MaxLimitType
            },
            RawDataSetting rawDataSetting => new Measurements.RawDataSetting { Extensions = MapExtensions(rawDataSetting.Extensions), Name = rawDataSetting.Name, Description = rawDataSetting.Description, Format = rawDataSetting.Format },
            _ => throw new NotSupportedException($"Element type {element.GetType()} is not supported.")
        };
    }
}
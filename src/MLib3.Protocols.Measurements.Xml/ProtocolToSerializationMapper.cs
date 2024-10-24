namespace MLib3.Protocols.Measurements.Xml;

public static class ProtocolToSerializationMapper
{
    public static Protocol Map(IProtocol protocol)
    {
        var product = new Product
        {
            Extensions = MapExtensions(protocol.Product.Extensions),
            Material = protocol.Product.Material,
            MaterialText = protocol.Product.MaterialText,
            Order = protocol.Product.Order,
            Equipment = protocol.Product.Equipment
        };

        var meta = new Meta
        {
            Extensions = MapExtensions(protocol.Meta.Extensions),
            DeviceId = protocol.Meta.DeviceId,
            DeviceName = protocol.Meta.DeviceName,
            Software = protocol.Meta.Software,
            SoftwareVersion = protocol.Meta.SoftwareVersion,
            TestRoutine = protocol.Meta.TestRoutine,
            TestRoutineVersion = protocol.Meta.TestRoutineVersion,
            Timestamp = protocol.Meta.Timestamp,
            Type = protocol.Meta.Type,
            Operator = protocol.Meta.Operator
        };

        var results = new Results
        {
            Ok = protocol.Results.Ok,
            Extensions = MapExtensions(protocol.Results.Extensions),
            Data = protocol.Results.Data.Select(MapElement).ToList()
        };

        return new Protocol(product, meta, results);
    }

    private static Extensions? MapExtensions(IExtensions? extensions)
    {
        if (extensions is null)
            return null;

        var result = new Extensions();
        foreach (var extension in extensions)
            result.Add(new Extension(extension.Key, extension.Value));
        return result;
    }

    private static Element MapElement(IElement element)
    {
        // switch-case for different element types
        return element switch
        {
            Measurements.Section section => new Section
            {
                Extensions = MapExtensions(section.Extensions),
                Name = section.Name,
                Description = section.Description,
                Data = section.Data.Select(MapElement).ToList(),
                Ok = section.Ok
            },
            Measurements.Comment comment => new Comment { Extensions = MapExtensions(comment.Extensions), Name = comment.Name, Description = comment.Description, Text = comment.Text },
            Measurements.Info info => new Info { Extensions = MapExtensions(info.Extensions), Name = info.Name, Description = info.Description, Value = info.Value },
            Measurements.Flag flag => new Flag
            {
                Extensions = MapExtensions(flag.Extensions), Name = flag.Name, Description = flag.Description, Ok = flag.Ok
            },
            Measurements.Value value => new Value
            {
                Extensions = MapExtensions(value.Extensions),
                Name = value.Name,
                Description = value.Description,
                Unit = value.Unit,
                Precision = value.Precision.GetValueOrDefault(),
                PrecisionSpecified = value.Precision.HasValue,
                Min = value.Min,
                Nom = value.Nom,
                Max = value.Max,
                MinLimitType = value.MinLimitType,
                MaxLimitType = value.MaxLimitType,
                Result = value.Result,
                Ok = value.Ok
            },
            Measurements.RawData rawData => new RawData
            {
                Extensions = MapExtensions(rawData.Extensions),
                Name = rawData.Name,
                Description = rawData.Description,
                Format = rawData.Format,
                Raw = rawData.Raw
            },
            Measurements.CommentSetting commentSetting => new CommentSetting { Extensions = MapExtensions(commentSetting.Extensions), Name = commentSetting.Name, Description = commentSetting.Description },
            Measurements.InfoSetting infoSetting => new InfoSetting
            {
                Extensions = MapExtensions(infoSetting.Extensions),
                Name = infoSetting.Name,
                Description = infoSetting.Description,
                Precision = infoSetting.Precision.GetValueOrDefault(),
                PrecisionSpecified = infoSetting.Precision.HasValue,
                Unit = infoSetting.Unit
            },
            Measurements.FlagSetting flagSetting => new FlagSetting { Extensions = MapExtensions(flagSetting.Extensions), Name = flagSetting.Name, Description = flagSetting.Description },
            Measurements.ValueSetting valueSetting => new ValueSetting
            {
                Extensions = MapExtensions(valueSetting.Extensions),
                Name = valueSetting.Name,
                Description = valueSetting.Description,
                Precision = valueSetting.Precision.GetValueOrDefault(),
                PrecisionSpecified = valueSetting.Precision.HasValue,
                Unit = valueSetting.Unit,
                Min = valueSetting.Min,
                Nom = valueSetting.Nom,
                Max = valueSetting.Max,
                MinLimitType = valueSetting.MinLimitType,
                MaxLimitType = valueSetting.MaxLimitType
            },
            Measurements.RawDataSetting rawDataSetting => new RawDataSetting { Extensions = MapExtensions(rawDataSetting.Extensions), Name = rawDataSetting.Name, Description = rawDataSetting.Description, Format = rawDataSetting.Format },
            _ => throw new NotSupportedException($"Element type {element.GetType()} is not supported.")
        };
    }
}
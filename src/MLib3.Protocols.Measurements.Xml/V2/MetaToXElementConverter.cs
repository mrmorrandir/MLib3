namespace MLib3.Protocols.Measurements.Xml.V2;

internal class MetaToXElementConverter : IConverter<IMeta, XElement>
{
    private readonly IConverter<IExtensions, XElement> _extensionsToXElementConverter;

    public MetaToXElementConverter(IConverter<IExtensions, XElement> extensionsToXElementConverter)
    {
        _extensionsToXElementConverter = extensionsToXElementConverter;
    }
    public XElement Convert(IMeta input)
    {
        var meta = new XElement("MeasurementMeta",
            new XAttribute("Type", input.Type),
            new XElement("Timestamp", input.Timestamp));
        if (input.DeviceName != null)
            meta.Add(new XElement("DeviceName", input.DeviceName));
        if (input.DeviceId != null)
            meta.Add(new XElement("DeviceID", input.DeviceId));
        if (input.Operator != null)
            meta.Add(new XElement("Worker", input.Operator));
        if (input.Software != null)
            meta.Add(new XElement("Program", input.Software));
        if (input.SoftwareVersion != null)
            meta.Add(new XElement("ProgramVersion", input.SoftwareVersion));
        if (input.TestRoutine != null)
            meta.Add(new XElement("TestRoutine", input.TestRoutine));
        if (input.TestRoutineVersion != null)
            meta.Add(new XElement("TestRoutineVersion", input.TestRoutineVersion));
        if (input.Extensions != null)
            meta.Add(_extensionsToXElementConverter.Convert(input.Extensions));
        return meta;
    }
}
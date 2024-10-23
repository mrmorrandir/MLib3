namespace MLib3.Protocols.Measurements.Xml.V3;

public interface IProtocolToStringConverter : IConverter<IProtocol, string>
{
    string Convert(IProtocol input);
}
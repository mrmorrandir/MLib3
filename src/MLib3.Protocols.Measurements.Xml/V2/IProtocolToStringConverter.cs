namespace MLib3.Protocols.Measurements.Xml.V2;

public interface IProtocolToStringConverter : IConverter<IProtocol, string>
{
    string Convert(IProtocol input);
}
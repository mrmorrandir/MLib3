namespace MLib3.Protocols.Measurements.Xml;

public interface ISerializationPostProcessor
{
    string Process(string xml);
}
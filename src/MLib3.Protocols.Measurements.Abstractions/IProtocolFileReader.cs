namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProtocolFileReader
{
    IProtocol Read(string filename);
}
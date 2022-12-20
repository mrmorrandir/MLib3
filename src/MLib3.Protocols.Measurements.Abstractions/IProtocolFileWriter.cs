namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProtocolFileWriter
{
    void Write(string filename, IProtocol protocol);
}
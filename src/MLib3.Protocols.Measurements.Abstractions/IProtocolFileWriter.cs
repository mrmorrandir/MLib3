using FluentResults;

namespace MLib3.Protocols.Measurements;

public interface IProtocolFileWriter
{
    Result Write(string filename, IProtocol protocol);
}
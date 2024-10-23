using FluentResults;

namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProtocolFileWriter
{
    Result Write(string filename, IProtocol protocol);
}
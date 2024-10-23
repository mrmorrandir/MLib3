using FluentResults;

namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProtocolFileReader
{
    Result<IProtocol> Read(string filename);
}
using FluentResults;

namespace MLib3.Protocols.Measurements;

public interface IProtocolFileReader
{
    Result<IProtocol> Read(string filename);
}
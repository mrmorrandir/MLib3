using System.IO.Abstractions;
using FluentResults;
using MLib3.Protocols.Measurements.Xml.V3;

namespace MLib3.Protocols.Measurements.Xml;

public class ProtocolFileWriter : IProtocolFileWriter
{
    private readonly IFileSystem _fileSystem;
    private readonly IProtocolSerializer _protocolSerializer;

    public ProtocolFileWriter(IFileSystem fileSystem, IProtocolSerializer protocolSerializer)
    {
        _fileSystem = fileSystem;
        _protocolSerializer = protocolSerializer;
    }
    
    public Result Write(string filename, IProtocol protocol)
    {
        var serializeResult = _protocolSerializer.Serialize(protocol);
        if (serializeResult.IsFailed)
            return serializeResult.ToResult();

        return Result.Try(() => _fileSystem.File.WriteAllText(filename, serializeResult.Value));
    }
}

using System.IO.Abstractions;
using FluentResults;

namespace MLib3.Protocols.Measurements.Xml;

public class ProtocolFileReader : IProtocolFileReader
{
    private readonly IFileSystem _fileSystem;
    private readonly IProtocolDeserializer _protocolDeserializer;

    public ProtocolFileReader(IFileSystem fileSystem, IProtocolDeserializer protocolDeserializer)
    {
        _fileSystem = fileSystem;
        _protocolDeserializer = protocolDeserializer;
    }
    
    public Result<IProtocol> Read(string filename)
    {
        if (!_fileSystem.File.Exists(filename))
            return Result.Fail($"File '{filename}' does not exist");

        var readResult = Result.Try(() => _fileSystem.File.ReadAllText(filename));
        if (readResult.IsFailed)
            return readResult.ToResult();

        return _protocolDeserializer.Deserialize(readResult.Value);
    }
}
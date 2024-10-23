using FluentResults;

namespace MLib3.Protocols.Measurements.Xml;

public class ProtocolDeserializer : IProtocolDeserializer
{
    public ProtocolDeserializer()
    {
        
    }
    
    public Result<IProtocol> Deserialize(string json)
    {
        throw new NotImplementedException();
    }
}
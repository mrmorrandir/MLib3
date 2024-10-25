namespace MLib3.Protocols.Measurements;

public interface IProtocol
{
    IProduct Product { get; }
    IMeta Meta { get; }
    IResults Results { get; }

    string Specification { get; }
    string Version { get; }
}
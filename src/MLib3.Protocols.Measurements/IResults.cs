namespace MLib3.Protocols.Measurements;

public interface IResults : IEvaluable
{
    bool Ok { get; set; }
    List<Element> Data { get; set; }
    Extensions? Extensions { get; set; }
}
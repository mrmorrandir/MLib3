namespace MLib3.Protocols.Measurements;

public interface IValue : IElement, IValueSetting, IEvaluated
{
    double Result { get; set; }
}
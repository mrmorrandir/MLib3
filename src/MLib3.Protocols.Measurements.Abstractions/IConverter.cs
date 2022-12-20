namespace MLib3.Protocols.Measurements.Abstractions;

public interface IConverter<in TInput, out TOutput>
{
    public TOutput Convert(TInput input);
}
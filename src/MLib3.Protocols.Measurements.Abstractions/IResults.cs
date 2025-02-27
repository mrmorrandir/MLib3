﻿namespace MLib3.Protocols.Measurements;

public interface IResults : IExtendable, IEvaluated
{
    IList<IElement> Data { get; }
    IElement this[string name] { get; }

    IResults Add(IElement element);
    IResults AddRange(IEnumerable<IElement> elements);
    IResults AddRange(IResults results);

    IResults Remove(IElement element);
    IResults RemoveRange(IEnumerable<IElement> elements);
    IResults RemoveRange(IResults results);

    IResults Clear();
}
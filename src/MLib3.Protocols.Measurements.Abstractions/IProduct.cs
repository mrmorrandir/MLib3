﻿namespace MLib3.Protocols.Measurements.Abstractions;

public interface IProduct : IExtendable
{
    string Equipment { get; }
    string? Material { get; }
    string? MaterialText { get; }
    string? Order { get; }
    
}
﻿namespace MLib3.Protocols.Measurements.Abstractions;

public interface IValue : IElement, IValueSetting, IEvaluated
{
    double Result { get; set; }
}
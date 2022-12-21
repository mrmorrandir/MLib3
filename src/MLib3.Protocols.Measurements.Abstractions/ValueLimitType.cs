namespace MLib3.Protocols.Measurements.Abstractions;

public enum ValueLimitType
{
    /// <summary>
    ///     No Information
    /// </summary>
    None,

    /// <summary>
    ///     Tolerated Limit
    /// </summary>
    Value,

    /// <summary>
    ///     Natural Limit (eg. 0 dB for Noise)
    /// </summary>
    Natural
}
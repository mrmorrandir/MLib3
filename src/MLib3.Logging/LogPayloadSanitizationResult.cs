namespace MLib3.Logging;

/// <summary>
/// Represents the outcome of sanitizing a logging payload.
/// </summary>
public sealed record LogPayloadSanitizationResult
{
    private LogPayloadSanitizationResult(bool isSuccess, bool isExcluded, string? payload)
    {
        IsSuccess = isSuccess;
        IsExcluded = isExcluded;
        Payload = payload;
    }

    public bool IsSuccess { get; }

    public bool IsExcluded { get; }

    public string? Payload { get; }

    public static LogPayloadSanitizationResult Success(string payload) => new(true, false, payload);

    public static LogPayloadSanitizationResult Excluded() => new(true, true, null);

    public static LogPayloadSanitizationResult Failed() => new(false, false, null);
}

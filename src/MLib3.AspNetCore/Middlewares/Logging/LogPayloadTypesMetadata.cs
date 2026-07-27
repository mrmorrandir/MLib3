namespace MLib3.AspNetCore;

/// <summary>
/// Explicitly identifies request and response payload types for API log sanitization.
/// </summary>
public sealed record LogPayloadTypesMetadata(Type? RequestType, Type? ResponseType);

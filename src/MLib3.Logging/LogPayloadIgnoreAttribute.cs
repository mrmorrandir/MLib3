namespace MLib3.Logging;

/// <summary>
/// Excludes a property or an entire type from logging payloads.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class LogPayloadIgnoreAttribute : Attribute
{
}

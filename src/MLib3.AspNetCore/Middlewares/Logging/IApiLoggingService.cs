namespace MLib3.AspNetCore;

/// <summary>
/// Dispatches API log entries to registered <see cref="IApiLogHandler"/> implementations.
/// </summary>
public interface IApiLoggingService
{
    /// <summary>
    /// Dispatches a captured API log entry to all registered handlers.
    /// </summary>
    /// <param name="log">The captured API log entry.</param>
    /// <param name="cancellationToken">A token that can be used to cancel asynchronous work.</param>
    /// <returns>A task representing the asynchronous dispatch operation.</returns>
    Task LogAsync(ApiLog log, CancellationToken cancellationToken = default);
}
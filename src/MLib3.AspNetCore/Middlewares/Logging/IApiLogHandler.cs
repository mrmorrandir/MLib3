namespace MLib3.AspNetCore;

/// <summary>
/// Handles API log entries created by <see cref="ApiLoggingMiddleware"/>.
/// </summary>
/// <remarks>
/// Implement this interface to enrich, forward, or persist API logs, for example in a database.
/// Implementations are resolved from the application's dependency injection container.
/// </remarks>
public interface IApiLogHandler
{
    /// <summary>
    /// Handles a captured API log entry.
    /// </summary>
    /// <param name="log">The captured API log entry.</param>
    /// <param name="cancellationToken">A token that can be used to cancel asynchronous work.</param>
    /// <returns>A task representing the asynchronous handling operation.</returns>
    Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default);
}
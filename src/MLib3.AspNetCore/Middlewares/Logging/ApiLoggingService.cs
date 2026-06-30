namespace MLib3.AspNetCore;

/// <summary>
/// Default implementation of <see cref="IApiLoggingService"/> that dispatches captured API logs to registered handlers.
/// </summary>
/// <remarks>
/// The service itself does not persist or buffer log entries. It forwards each <see cref="ApiLog"/> to all
/// registered <see cref="IApiLogHandler"/> implementations in registration order. If no handlers are registered,
/// dispatching a log entry is a no-op.
/// </remarks>
internal sealed class ApiLoggingService : IApiLoggingService
{
    private readonly IApiLogHandler[] _handlers;
    private readonly ILogger<ApiLoggingService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiLoggingService"/> class.
    /// </summary>
    /// <param name="handlers">The registered API log handlers that should receive captured log entries.</param>
    /// <param name="logger">The logger used to report handler failures.</param>
    public ApiLoggingService(IEnumerable<IApiLogHandler> handlers, ILogger<ApiLoggingService> logger)
    {
        _handlers = handlers.ToArray();
        _logger = logger;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Handler exceptions are caught and logged so that a failing log handler does not interrupt the HTTP request
    /// pipeline. The provided cancellation token is passed to each handler.
    /// </remarks>
    public async Task LogAsync(ApiLog log, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(log);

        foreach (var handler in _handlers)
        {
            try
            {
                await handler.HandleAsync(log, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API log handler {HandlerType} failed", handler.GetType().FullName);
            }
        }
    }
}

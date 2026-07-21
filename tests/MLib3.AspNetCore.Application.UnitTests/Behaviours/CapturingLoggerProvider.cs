using Microsoft.Extensions.Logging;

namespace MLib3.AspNetCore.Application.UnitTests.Behaviours;

internal sealed class CapturingLoggerProvider : ILoggerProvider
{
    public List<LogEntry> Entries { get; } = [];

    public ILogger<T> CreateLogger<T>() => new CapturingLogger<T>(CreateLogger(typeof(T).FullName ?? typeof(T).Name));

    public ILogger CreateLogger(string categoryName) => new CapturingLogger(categoryName, Entries);

    public void Dispose()
    {
    }

    internal sealed record LogEntry(
        string CategoryName,
        LogLevel LogLevel,
        string Message,
        Exception? Exception);

    private sealed class CapturingLogger(string categoryName, List<LogEntry> entries) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull =>
            null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            entries.Add(new LogEntry(categoryName, logLevel, formatter(state, exception), exception));
        }
    }

    private sealed class CapturingLogger<T>(ILogger logger) : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull =>
            logger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => logger.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}

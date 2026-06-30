using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MLib3.AspNetCore.UnitTests;

public class ApiLoggingServiceTests
{
    [Fact]
    public async Task LogAsync_WhenNoHandlersAreRegistered_CompletesWithoutError()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApiLoggingMiddleware();

        await using var serviceProvider = services.BuildServiceProvider();
        var apiLoggingService = serviceProvider.GetRequiredService<IApiLoggingService>();

        Func<Task> act = () => apiLoggingService.LogAsync(CreateApiLog());

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task LogAsync_WhenHandlersAreRegistered_DispatchesLogToEveryHandler()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<CapturedApiLogStore>();
        services.AddApiLoggingMiddleware();
        services.AddApiLogHandler<FirstRecordingApiLogHandler>();
        services.AddApiLogHandler<SecondRecordingApiLogHandler>();

        await using var serviceProvider = services.BuildServiceProvider();
        var apiLoggingService = serviceProvider.GetRequiredService<IApiLoggingService>();
        var store = serviceProvider.GetRequiredService<CapturedApiLogStore>();
        var log = CreateApiLog();

        await apiLoggingService.LogAsync(log);

        store.Entries.Should().Equal(
            new CapturedApiLog("First", log),
            new CapturedApiLog("Second", log));
    }

    [Fact]
    public async Task LogAsync_WhenHandlerThrows_LogsErrorAndContinuesWithRemainingHandlers()
    {
        var loggerProvider = new CapturingLoggerProvider();
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddProvider(loggerProvider));
        services.AddSingleton<CapturedApiLogStore>();
        services.AddApiLoggingMiddleware();
        services.AddApiLogHandler<FirstRecordingApiLogHandler>();
        services.AddApiLogHandler<ThrowingApiLogHandler>();
        services.AddApiLogHandler<SecondRecordingApiLogHandler>();

        await using var serviceProvider = services.BuildServiceProvider();
        var apiLoggingService = serviceProvider.GetRequiredService<IApiLoggingService>();
        var store = serviceProvider.GetRequiredService<CapturedApiLogStore>();
        var log = CreateApiLog();

        await apiLoggingService.LogAsync(log);

        store.Entries.Should().Equal(
            new CapturedApiLog("First", log),
            new CapturedApiLog("Second", log));

        loggerProvider.Entries.Should().ContainSingle(entry =>
            entry.CategoryName == "MLib3.AspNetCore.Logging.ApiLoggingService"
            && entry.LogLevel == LogLevel.Error
            && entry.Message == "API log handler MLib3.AspNetCore.UnitTests.ApiLoggingServiceTests+ThrowingApiLogHandler failed");
    }

    private static ApiLog CreateApiLog() =>
        new()
        {
            Timestamp = DateTime.UtcNow,
            Method = "POST",
            Path = "/api/users",
            QueryString = "?include=details",
            RequestJson = """{"name":"Ada"}""",
            ResponseJson = """{"id":42}""",
            StatusCode = 201,
            DurationMilliseconds = 12
        };

    private sealed class CapturedApiLogStore
    {
        public List<CapturedApiLog> Entries { get; } = [];
    }

    private sealed record CapturedApiLog(string HandlerName, ApiLog Log);

    private sealed class FirstRecordingApiLogHandler(CapturedApiLogStore store) : IApiLogHandler
    {
        public Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default)
        {
            store.Entries.Add(new CapturedApiLog("First", log));
            return Task.CompletedTask;
        }
    }

    private sealed class SecondRecordingApiLogHandler(CapturedApiLogStore store) : IApiLogHandler
    {
        public Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default)
        {
            store.Entries.Add(new CapturedApiLog("Second", log));
            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingApiLogHandler : IApiLogHandler
    {
        public Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("Handler failed.");
    }

    private sealed class CapturingLoggerProvider : ILoggerProvider
    {
        public List<LogEntry> Entries { get; } = [];

        public ILogger CreateLogger(string categoryName) => new CapturingLogger(categoryName, Entries);

        public void Dispose()
        {
        }
    }

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

    private sealed record LogEntry(
        string CategoryName,
        LogLevel LogLevel,
        string Message,
        Exception? Exception);
}

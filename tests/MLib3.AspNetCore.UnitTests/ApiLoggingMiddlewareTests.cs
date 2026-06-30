using System.Text;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MLib3.AspNetCore.UnitTests;

public class ApiLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenEnabled_LogsRequestAndResponseDetails()
    {
        var logger = new CapturingLogger<ApiLoggingMiddleware>();
        var apiLoggingService = new CapturingApiLoggingService();
        var middleware = new ApiLoggingMiddleware(
            logger,
            apiLoggingService,
            Options.Create(new ApiLoggingOptions()));
        var context = CreateContext("""{"name":"Ada","age":37}""", "/api/users", "?include=details");
        var originalResponseBody = context.Response.Body;

        await middleware.InvokeAsync(
            context,
            async httpContext =>
            {
                httpContext.Response.StatusCode = StatusCodes.Status201Created;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync("""{"id":42,"created":true}""");
            });

        logger.Entries.Should().ContainSingle();
        var entry = logger.Entries.Single();

        entry.LogLevel.Should().Be(LogLevel.Information);
        entry.Message.Should().StartWith("API POST /api/users responded 201 in ");
        entry.Message.Should().EndWith("""ms - Request: {"name":"Ada","age":37} Response: {"id":42,"created":true}""");

        entry.Properties["Method"].Should().Be("POST");
        entry.Properties["Path"].Should().Be("/api/users");
        entry.Properties["StatusCode"].Should().Be(StatusCodes.Status201Created);
        entry.Properties["Duration"].Should().BeOfType<long>();
        entry.Properties["RequestJson"]!.ToString().Should().Be("""{"name":"Ada","age":37}""");
        entry.Properties["Response"]!.ToString().Should().Be("""{"id":42,"created":true}""");
        entry.Properties["{OriginalFormat}"].Should().Be(
            "API {Method} {Path} responded {StatusCode} in {Duration}ms - Request: {RequestJson} Response: {Response}");

        apiLoggingService.Logs.Should().ContainSingle();
        var apiLog = apiLoggingService.Logs.Single();
        apiLog.Method.Should().Be("POST");
        apiLog.Path.Should().Be("/api/users");
        apiLog.QueryString.Should().Be("?include=details");
        apiLog.RequestJson.Should().Be("""{"name":"Ada","age":37}""");
        apiLog.ResponseJson.Should().Be("""{"id":42,"created":true}""");
        apiLog.StatusCode.Should().Be(StatusCodes.Status201Created);
        apiLog.DurationMilliseconds.Should().BeGreaterThanOrEqualTo(0);

        originalResponseBody.Position = 0;
        var responseBody = await new StreamReader(originalResponseBody).ReadToEndAsync();
        responseBody.Should().Be("""{"id":42,"created":true}""");
    }

    [Fact]
    public async Task InvokeAsync_WhenLogLevelIsNone_DoesNotLog()
    {
        var logger = new CapturingLogger<ApiLoggingMiddleware>(LogLevel.None);
        var apiLoggingService = new CapturingApiLoggingService();
        var middleware = new ApiLoggingMiddleware(
            logger,
            apiLoggingService,
            Options.Create(new ApiLoggingOptions()));
        var context = CreateContext("", "/api/silent", "");
        context.Request.Method = HttpMethods.Get;

        await middleware.InvokeAsync(
            context,
            async httpContext =>
            {
                await httpContext.Response.WriteAsync("Ignored");
            });

        logger.Entries.Should().BeEmpty();
        apiLoggingService.Logs.Should().BeEmpty();
    }

    [Fact]
    public async Task InvokeAsync_WhenPathIsExcluded_DoesNotLog()
    {
        var logger = new CapturingLogger<ApiLoggingMiddleware>();
        var apiLoggingService = new CapturingApiLoggingService();
        var middleware = new ApiLoggingMiddleware(
            logger,
            apiLoggingService,
            Options.Create(new ApiLoggingOptions
            {
                ExcludedPaths = ["/api/internal"]
            }));
        var context = CreateContext("""{"secret":true}""", "/API/Internal/status", "");

        await middleware.InvokeAsync(
            context,
            async httpContext =>
            {
                httpContext.Response.StatusCode = StatusCodes.Status202Accepted;
                await httpContext.Response.WriteAsync("Skipped path");
            });

        logger.Entries.Should().BeEmpty();
        apiLoggingService.Logs.Should().BeEmpty();

        context.Response.Body.Position = 0;
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        responseBody.Should().Be("Skipped path");
        context.Response.StatusCode.Should().Be(StatusCodes.Status202Accepted);
    }

    [Fact]
    public async Task InvokeAsync_WhenFileIsExcluded_DoesNotLog()
    {
        var logger = new CapturingLogger<ApiLoggingMiddleware>();
        var apiLoggingService = new CapturingApiLoggingService();
        var middleware = new ApiLoggingMiddleware(
            logger,
            apiLoggingService,
            Options.Create(new ApiLoggingOptions
            {
                ExcludedFiles = ["/assets/hallo*.JS"]
            }));
        var context = CreateContext("", "/assets/Hallo-Welt.js", "");
        context.Request.Method = HttpMethods.Get;

        await middleware.InvokeAsync(
            context,
            async httpContext =>
            {
                await httpContext.Response.WriteAsync("Skipped file");
            });

        logger.Entries.Should().BeEmpty();
        apiLoggingService.Logs.Should().BeEmpty();

        context.Response.Body.Position = 0;
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        responseBody.Should().Be("Skipped file");
    }

    private sealed class CapturingApiLoggingService : IApiLoggingService
    {
        public List<ApiLog> Logs { get; } = [];

        public Task LogAsync(ApiLog log, CancellationToken cancellationToken = default)
        {
            Logs.Add(log);
            return Task.CompletedTask;
        }
    }

    private static DefaultHttpContext CreateContext(string requestJson, string path, string queryString)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Path = path;
        context.Request.QueryString = new QueryString(queryString);
        context.Request.ContentType = "application/json";
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestJson));
        context.Response.Body = new MemoryStream();
        return context;
    }

    private sealed class CapturingLogger<T>(LogLevel minLevel = LogLevel.Trace) : ILogger<T>
    {
        public List<LogEntry> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull =>
            null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None && logLevel >= minLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Entries.Add(new LogEntry(
                logLevel,
                formatter(state, exception),
                GetProperties(state)));
        }

        private static Dictionary<string, object?> GetProperties<TState>(TState state)
        {
            if (state is not IEnumerable<KeyValuePair<string, object?>> properties)
            {
                return [];
            }

            return properties.ToDictionary(x => x.Key, x => x.Value);
        }
    }

    private sealed record LogEntry(
        LogLevel LogLevel,
        string Message,
        Dictionary<string, object?> Properties);
}

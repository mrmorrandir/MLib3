# MLib3.AspNetCore

MLib3.AspNetCore is a library that provides extension methods for converting `FluentResults` to web-compatible results and error responses in ASP.NET Core applications.

## Installation

You can install the package via NuGet:

```bash
dotnet add package MLib3.AspNetCore
```

## Usage

### ResultExtensions

The `ResultExtensions` class provides `ToWebResult` extension methods for `Result`, `Task<Result>`, and `ValueTask<Result>` (both generic and non-generic versions). These methods simplify returning `IResult` from Minimal APIs or Controllers when using `FluentResults`.

#### Minimal API Example

```csharp
using MLib3.AspNetCore;
using FluentResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/items/{id}", async (int id, IItemService service) => 
{
    // service.GetItem returns Task<Result<Item>>
    return await service.GetItem(id).ToWebResult();
});

app.MapPost("/items", async (CreateItemCommand command, IMediator mediator) => 
{
    // mediator.Send returns Task<Result>
    return await mediator.Send(command).ToWebResult();
});

app.Run();
```

#### Behavior

- **Success**: Returns `Results.Ok(value)` for `Result<T>` or `Results.Ok()` for `Result`.
- **Failure**: Returns `Results.BadRequest(ErrorResponse)` where `ErrorResponse` contains a structured list of errors extracted from the `FluentResult`.

#### Error Response Structure

When a result fails, the library returns a structured error response:

```json
{
  "errors": [
    {
      "message": "Error message",
      "causedBy": [],
      "meta": [
        { "key": "SomeKey", "value": "SomeValue" }
      ]
    }
  ]
}
```

### Endpoints

The endpoint helpers allow you to organize Minimal API route registrations in small endpoint classes instead of registering all routes directly in `Program.cs`.

Implement the `IEndpoint` interface for each endpoint:

```csharp
using MLib3.AspNetCore;

public sealed class GetItemEndpoint : IEndpoint
{
    public void Register(WebApplication app)
    {
        app.MapGet("/items/{id}", async (int id, IItemService service) =>
        {
            return await service.GetItem(id).ToWebResult();
        });
    }
}
```

Register endpoint implementations in the service collection and map them during application startup:

```csharp
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();

var app = builder.Build();

app.UseEndpoints();

app.Run();
```

#### Endpoint Registration Behavior

- `AddEndpoints()` scans the entry assembly when no assemblies are provided.
- `AddEndpoints(params Assembly[] assemblies)` scans the provided assemblies for concrete, non-abstract `IEndpoint` implementations.
- Each endpoint implementation is registered as a singleton `IEndpoint`.
- `UseEndpoints()` resolves all registered `IEndpoint` services and calls `Register(app)` on each implementation.

To scan endpoints from another assembly, pass it explicitly:

```csharp
using System.Reflection;

builder.Services.AddEndpoints(typeof(GetItemEndpoint).Assembly);
```

### Middlewares

#### ExceptionHandlingMiddleware

`ExceptionHandlingMiddleware` is a last line of defense for unhandled exceptions in the ASP.NET Core request pipeline. It catches exceptions thrown by later middleware or endpoints, logs them, and returns the same structured error response format used by `ResultExtensions`.

Add it to the request pipeline with `UseExceptionHandlingMiddleware()`:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseEndpoints();

app.Run();
```

#### Middleware Behavior

- Catches unhandled exceptions from later pipeline steps.
- Logs the exception with `ILogger<ExceptionHandlingMiddleware>`.
- Sets the response status code to `400 Bad Request`.
- Sets the response content type to `application/json`.
- Returns an `ErrorResponse` created from a `FluentResults.ExceptionalError`.

Example response:

```json
{
  "errors": [
    {
      "message": "An exception occurred",
      "causedBy": [],
      "meta": [
        { "key": "Exception", "value": "System.InvalidOperationException: ..." }
      ]
    }
  ]
}
```

#### ApiLoggingMiddleware

`ApiLoggingMiddleware` captures API request and response details and logs them with `ILogger<ApiLoggingMiddleware>`.
It can also dispatch the captured `ApiLog` entries to custom `IApiLogHandler` implementations so applications can
enrich, forward, or persist API logs.

Register and use the middleware during application startup:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLoggingMiddleware();

var app = builder.Build();

app.UseApiLoggingMiddleware();

app.Run();
```

Captured `ApiLog` entries contain:

- `Timestamp`
- `Method`
- `Path`
- `QueryString`
- `RequestJson`
- `ResponseJson`
- `StatusCode`
- `DurationMilliseconds`

##### Sensitive Logging Payloads

The middleware sanitizes JSON request and response bodies before they are passed to
`ILogger<ApiLoggingMiddleware>` or any `IApiLogHandler`. The original HTTP request and response bodies are not
changed.

Use `LogRedactAttribute` on a property to retain its JSON property name while replacing its value with
`"[REDACTED]"`. A custom replacement can be passed to the attribute. Use `LogPayloadIgnoreAttribute` on a property
to remove that property from the logging payload, or on a class or record to omit the complete object payload.
These attributes only affect logging. They do not affect request binding, HTTP responses, or global
`System.Text.Json` behavior.

```csharp
using MLib3.Logging;

public sealed record AuthRequest(
    string Username,
    [property: LogRedact] string Password);

public sealed record AuthResponse(
    [property: LogRedact] string Token,
    DateTimeOffset ValidUntil);
```

Redaction is recursive and applies to nested objects, collections, dictionaries, and known FluentResults payloads.
If sanitization or JSON parsing fails, the middleware omits the affected payload and writes a technical warning
without including its contents.

##### How Payload Types Are Resolved

Attributes can only be applied to an HTTP body when the middleware knows its C# DTO type. The middleware first uses
explicit logging metadata and then uses standard ASP.NET Core endpoint metadata:

- Minimal API request DTOs are normally available through `IAcceptsMetadata` for JSON body parameters.
- Response DTOs are resolved through `IProducesResponseTypeMetadata`.
- When several response types are declared, the type matching the actual HTTP status code is selected.

Declare typed Minimal API responses with `Produces<TResponse>` so response attributes can be discovered:

```csharp
app.MapPost("/auth", HandleAuthentication)
    .Produces<AuthResponse>(StatusCodes.Status200OK);
```

No logging-specific extension is normally needed when the request is a regular JSON body parameter and every
response DTO is declared with `Produces<TResponse>`.

##### Explicit Payload Type Metadata

Use `LogPayloadEndpointConventionExtensions` when the standard endpoint metadata does not expose the real DTO type.
This commonly occurs when an endpoint returns `IResult`, uses a custom result or serializer, or does not declare its
response through `Produces<TResponse>`.

Declare only the missing response type when request metadata is already available:

```csharp
app.MapPost("/auth", (AuthRequest request) =>
        Results.Ok(new AuthResponse("real-token", DateTimeOffset.UtcNow)))
    .WithLogPayloadResponseType<AuthResponse>();
```

Declare only a missing request type with `WithLogPayloadRequestType<TRequest>()`. If neither type can be determined
reliably, declare both:

```csharp
app.MapPost("/auth", HandleAuthentication)
    .WithLogPayloadTypes<AuthRequest, AuthResponse>();
```

Request-only and response-only metadata can also be added separately; the resolver combines both declarations.
Explicit metadata has priority over automatically generated `Accepts` and `Produces` metadata.

When no DTO type is available, the middleware still recursively redacts configured sensitive JSON property names
such as `password`, `token`, and `authorization`, using case-insensitive matching. Attribute-based rules cannot be
discovered in that fallback mode, so endpoints with other sensitive property names must provide explicit payload
type metadata. Non-JSON bodies are omitted from payload logging.

##### Configuration

The middleware can be configured through the `ApiLogging` configuration section.
`ExcludedPaths` skips requests whose path starts with one of the configured values. Matching is case-insensitive.
`ExcludedFiles` supports `*` wildcards and is also matched case-insensitively.

```json
{
  "ApiLogging": {
    "ExcludedPaths": [
      "/health",
      "/swagger"
    ],
    "ExcludedFiles": [
      "/assets/*.js",
      "/favicon.ico"
    ]
  }
}
```

The same options can be configured in code:

```csharp
builder.Services.AddApiLoggingMiddleware(options =>
{
    options.WithExcludePaths("/health", "/swagger");
    options.WithExcludedFiles("/assets/*.js", "/favicon.ico");
});
```

##### Custom ApiLog Handlers

Implement `IApiLogHandler` to process captured API logs. A handler can write logs to a database, forward them to
another system, or apply application-specific filtering and enrichment.

```csharp
using MLib3.AspNetCore;

public sealed class DatabaseApiLogHandler : IApiLogHandler
{
    public async Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default)
    {
        // Persist or process the captured API log.
    }
}
```

Register the handler with the service collection:

```csharp
builder.Services.AddApiLoggingMiddleware();
builder.Services.AddApiLogHandler<DatabaseApiLogHandler>();
```

Multiple handlers can be registered. They are called in registration order. If no handler is registered, the
middleware still logs through `ILogger<ApiLoggingMiddleware>` and dispatching to handlers is a no-op. If a handler
throws an exception, the exception is logged and the request pipeline continues.

Handlers run during request processing. For slow database access or high traffic, asynchronous processing can be
useful: the handler can enqueue `ApiLog` entries quickly, while a hosted background service persists them later.

```csharp
using System.Threading.Channels;
using MLib3.AspNetCore;

public sealed class QueuedApiLogHandler : IApiLogHandler
{
    private readonly ChannelWriter<ApiLog> _writer;

    public QueuedApiLogHandler(ChannelWriter<ApiLog> writer)
    {
        _writer = writer;
    }

    public async Task HandleAsync(ApiLog log, CancellationToken cancellationToken = default)
    {
        await _writer.WriteAsync(log, cancellationToken);
    }
}

public sealed class ApiLogBackgroundWorker : BackgroundService
{
    private readonly ChannelReader<ApiLog> _reader;
    private readonly IServiceScopeFactory _scopeFactory;

    public ApiLogBackgroundWorker(ChannelReader<ApiLog> reader, IServiceScopeFactory scopeFactory)
    {
        _reader = reader;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var log in _reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();

            // Resolve a scoped DbContext or repository here and persist the ApiLog.
        }
    }
}
```

Register the queue, handler, and worker in the application:

```csharp
using System.Threading.Channels;
using MLib3.AspNetCore;

builder.Services.AddSingleton(Channel.CreateBounded<ApiLog>(1000));
builder.Services.AddSingleton(sp => sp.GetRequiredService<Channel<ApiLog>>().Reader);
builder.Services.AddSingleton(sp => sp.GetRequiredService<Channel<ApiLog>>().Writer);

builder.Services.AddApiLoggingMiddleware();
builder.Services.AddApiLogHandler<QueuedApiLogHandler>();
builder.Services.AddHostedService<ApiLogBackgroundWorker>();
```

## Integration with Mediator

As mentioned in the code remarks, this library works exceptionally well with `Mediator.SourceGenerator` when commands and queries return `FluentResults`.

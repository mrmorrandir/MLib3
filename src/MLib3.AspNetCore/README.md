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

## Integration with Mediator

As mentioned in the code remarks, this library works exceptionally well with `Mediator.SourceGenerator` when commands and queries return `FluentResults`.

# MLib3.AspNetCore.Application

`MLib3.AspNetCore.Application` provides application-layer helpers for ASP.NET Core projects that use
`FluentResults`, `FluentValidation`, `Mediator`, and Entity Framework Core.

The package contains:

- Mediator pipeline behaviours for logging, performance logging, validation, and unhandled exceptions.
- `IPagination` for common `Offset` / `Limit` pagination input.
- `PaginatedList<T>` for paginated query results.
- `ToPaginatedListAsync` extension methods for `IQueryable<T>`.

## Installation

```bash
dotnet add package MLib3.AspNetCore.Application
```

## Dependencies

The project targets `net10.0` and references:

- `FluentResults`
- `FluentValidation`
- `Mediator.Abstractions`
- `Microsoft.EntityFrameworkCore`
- `MLib3.Logging`

Applications that use the Mediator behaviours must also configure Mediator in the consuming application. The
`AddMediator` example below requires Mediator source-generator setup in that application.

## Mediator Pipeline Behaviours

The behaviours are in the `MLib3.AspNetCore.Application.Behaviours` namespace.

```csharp
using Mediator;
using MLib3.AspNetCore.Application.Behaviours;

builder.Services.AddMediator(options =>
{
    options.Assemblies = [typeof(Program).Assembly];
    options.PipelineBehaviors =
    [
        typeof(LoggingBehaviour<,>),
        typeof(PerformanceBehaviour<,>),
        typeof(UnhandledExceptionBehaviour<,>),
        typeof(ValidationBehaviour<,>)
    ];
});
```

Register FluentValidation validators in the consuming application so that `ValidationBehaviour<TRequest, TResponse>`
can receive `IValidator<TRequest>` instances through dependency injection.

```csharp
using FluentValidation;

builder.Services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
```

### LoggingBehaviour

`LoggingBehaviour<TRequest, TResponse>` runs the next handler and logs sanitized JSON representations of the request
and response. Register the shared sanitizer before resolving the behaviour:

```csharp
builder.Services.AddLogPayloadSanitizer();
```

- `TRequest` must implement `Mediator.IMessage`.
- `TResponse` must implement `FluentResults.IResultBase`.
- Failed responses are logged as warnings.
- Successful responses are logged as information.
- Request type, result status, and execution duration remain visible when a payload is excluded.

#### Sensitive Logging Payloads

Use `LogRedactAttribute` on properties whose names may remain visible while their values must be replaced. The
default replacement is `"[REDACTED]"`; a custom replacement can be passed to the attribute.

Use `LogPayloadIgnoreAttribute` on:

- A property to remove that property from the logging payload.
- A class or record to omit the complete object payload.

```csharp
using FluentResults;
using Mediator;
using MLib3.Logging;

public sealed record AuthUserCommand(
    string Username,
    [property: LogRedact] string Password)
    : ICommand<Result<AuthResult>>;

public sealed record AuthResult(
    [property: LogRedact] string Token,
    DateTimeOffset ValidUntil);
```

The attributes affect only the representation created for logging. They do not modify the original request or
response objects, change FluentResults, or alter global `System.Text.Json` behavior.

Sanitization is recursive and supports records, regular classes, nullable values, collections, and dictionaries.
For `Result<T>`, a successful `Value` is sanitized recursively. A failed result is processed without accessing
`Value`. FluentResults errors, nested reasons, and `Error.Metadata` are also sanitized.

The sanitizer additionally redacts configured sensitive property or dictionary-key names using case-insensitive
matching. Its default defense-in-depth list includes names such as `password`, `token`, `accessToken`,
`refreshToken`, `secret`, `clientSecret`, and `authorization`. Application-specific names can be added during
registration:

```csharp
builder.Services.AddLogPayloadSanitizer(options =>
{
    options.SensitivePropertyNames.Add("apiKey");
});
```

If a request or response cannot be sanitized or serialized safely, `LoggingBehaviour` does not fall back to the
original object. It omits the affected payload and writes a technical warning without including payload contents.

`LoggingBehaviour` now requires `ILogPayloadSanitizer` through constructor injection. Applications that construct
the behaviour manually must provide it, and dependency-injection configurations must register it with
`AddLogPayloadSanitizer()`.

### PerformanceBehaviour

`PerformanceBehaviour<TRequest, TResponse>` measures handler execution time with a default warning threshold of
`1000` milliseconds.

- `TRequest` must implement `Mediator.IMessage`.
- Requests at or below the threshold are logged as debug entries.
- Requests above the threshold are logged as warnings.

### UnhandledExceptionBehaviour

`UnhandledExceptionBehaviour<TRequest, TResponse>` catches exceptions thrown by the next handler, logs them, and
returns a failed FluentResults response with an `ExceptionalError`.

- `TRequest` must implement `Mediator.IMessage`.
- `TResponse` must implement `FluentResults.IResultBase`.
- The implementation creates failures for `Result` and `Result<T>` response shapes.

### ValidationBehaviour

`ValidationBehaviour<TRequest, TResponse>` runs all registered `IValidator<TRequest>` validators.

- If no validators are registered, the request is passed to the next handler.
- If validation succeeds, the request is passed to the next handler.
- If validation fails, the handler is not called.
- Validation failures are returned as `Result.Fail(...)` with an `Error("Validation")` caused by the validation
  error messages.

Example request, handler, and validator:

```csharp
using FluentResults;
using FluentValidation;
using Mediator;

public sealed record CreateProductCommand(string Name) : ICommand<Result>;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result>
{
    public ValueTask<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(Result.Ok());
    }
}

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty();
    }
}
```

## Pagination

`IPagination` defines nullable `Offset` and `Limit` values:

```csharp
using MLib3.AspNetCore.Application;

public sealed record GetProductsQuery : IPagination
{
    public int? Offset { get; init; }
    public int? Limit { get; init; }
}
```

`PaginatedList<T>` contains:

- `Items`
- `Offset`
- `Limit`
- `Total`

`Total` is the result of `CountAsync` on the source query before `Skip` and `Take` are applied. Items are loaded with
`Skip(offset ?? 0)` and `Take(limit ?? total)`.

## QueryableExtensions

`QueryableExtensions` provides two overloads:

```csharp
Task<Result<PaginatedList<T>>> ToPaginatedListAsync<T>(
    this IQueryable<T> queryable,
    int? offset = null,
    int? limit = null,
    CancellationToken cancellationToken = default);

Task<Result<PaginatedList<T>>> ToPaginatedListAsync<T>(
    this IQueryable<T> queryable,
    IPagination pagination,
    CancellationToken cancellationToken = default);
```

Example with explicit `offset` and `limit`:

```csharp
using FluentResults;
using Microsoft.EntityFrameworkCore;
using MLib3.AspNetCore.Application;

public async Task<Result<PaginatedList<Product>>> GetProductsAsync(CancellationToken cancellationToken)
{
    return await dbContext.Products
        .AsNoTracking()
        .ToPaginatedListAsync(offset: 0, limit: 20, cancellationToken);
}
```

Minimal API query binding can use a concrete type that implements `IPagination`:

```csharp
using MLib3.AspNetCore.Application;

public sealed record ProductsRequest : IPagination
{
    public int? Offset { get; init; }
    public int? Limit { get; init; }
}

app.MapGet("/products", async ([AsParameters] ProductsRequest request, CancellationToken cancellationToken) =>
{
    var result = await dbContext.Products
        .AsNoTracking()
        .ToPaginatedListAsync(request, cancellationToken);

    return result.ToWebResult();
});
```

For HTTP result conversion, combine the returned `Result<PaginatedList<T>>` with the `ToWebResult()` extensions from
`MLib3.AspNetCore`.

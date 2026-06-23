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

## Integration with Mediator

As mentioned in the code remarks, this library works exceptionally well with `Mediator.SourceGenerator` when commands and queries return `FluentResults`.

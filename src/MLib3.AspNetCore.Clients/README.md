# MLib3.AspNetCore.Clients

This package provides common client-side utilities for ASP.NET Core applications, specifically focusing on standardized error handling and integration with NSwag-generated clients.

## ApiClientException

The `ApiClientException` and its generic counterpart `ApiClientException<TResult>` are designed to be used as the base exception for NSwag-generated API clients. They provide easy access to HTTP status codes, response content, and headers.

### NSwagStudio Configuration

To use these exceptions with NSwagStudio, follow these steps:

1.  **Exclude Exception Generation**: In NSwagStudio, ensure that the option to generate exception classes is disabled (or configure it to use existing ones).
2.  **Base Exception Class**: Set the base exception class name to `ApiClientException`.
3.  **Namespace**: Ensure the namespace `MLib3.AspNetCore.Clients` is available in your client project (either via a `using` statement or by including it in the NSwag configuration).

By using `ApiClientException`, your client will throw these standardized exceptions, which can then be easily handled by the provided extension methods.

## Standardized Error Handling

The package integrates with `FluentResults` to provide a seamless way to convert API calls into result objects.

### TaskExtensions and ToResult()

You can use the `ToResult()` extension method on tasks returned by your API client. This method catches `ApiClientException<ErrorResponse>` and automatically converts the contained `ErrorResponse` into a `FluentResults.Result`.

#### Usage Example

```csharp
// Example using a generated NSwag client
var testResult = await client.GetAllAsync().ToResult();

if (testResult.IsSuccess)
{
    var data = testResult.Value;
    // Process data
}
else
{
    // Handle errors (e.g., log them or show them to the user)
    foreach (var error in testResult.Errors)
    {
        Console.WriteLine(error.Message);
    }
}
```

### ErrorResponse Mapping

If your API returns a standardized `ErrorResponse` (from `MLib3.AspNetCore`), the `ToResult()` method will:
1. Catch the `ApiClientException<ErrorResponse>`.
2. Extract the `ErrorResponse` object.
3. Map all `ErrorDetail` objects into `FluentResults.Error` objects, including their metadata and nested causes.

This allows for very clean and expressive service layers that don't need to manually catch exceptions for every API call.

using FluentResults;
using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys.Validators;

public class StringApiKeyValidator : IApiKeyValidator
{
    private readonly string _apiKey;

    public StringApiKeyValidator(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(apiKey));
        _apiKey = apiKey;
    }
    
    public Task<Result> ValidateAsync(string apiKey)
    {
        return apiKey == _apiKey
            ? Task.FromResult(Result.Ok())
            : Task.FromResult(Result.Fail("ApiKey is invalid"));
    }
}
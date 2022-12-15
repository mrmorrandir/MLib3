using FluentResults;
using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys;

public class StringApiKeyValidator : IApiKeyValidator
{
    private readonly string _apiKey;

    public StringApiKeyValidator(string apiKey)
    {
        _apiKey = apiKey;
    }
    
    public Task<Result> ValidateAsync(string apiKey)
    {
        return apiKey == _apiKey
            ? Task.FromResult(Result.Ok())
            : Task.FromResult(Result.Fail("ApiKey is invalid"));
    }
}
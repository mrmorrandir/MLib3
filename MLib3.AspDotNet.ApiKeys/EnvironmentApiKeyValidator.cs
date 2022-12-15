using FluentResults;
using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys;

public class EnvironmentApiKeyValidator : IApiKeyValidator
{
    private readonly string? _apiKey;

    public EnvironmentApiKeyValidator(string environmentVariable)
    {
        _apiKey = Environment.GetEnvironmentVariable(environmentVariable);
    }
    
    public Task<Result> ValidateAsync(string apiKey)
    {
        if (_apiKey is null) return Task.FromResult(Result.Ok());
        return apiKey == _apiKey
            ? Task.FromResult(Result.Ok())
            : Task.FromResult(Result.Fail("ApiKey is invalid"));
    }
}
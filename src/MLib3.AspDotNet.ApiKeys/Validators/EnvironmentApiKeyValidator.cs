using FluentResults;
using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys.Validators;

public class EnvironmentApiKeyValidator : IApiKeyValidator
{
    private readonly string _environmentVariableName;

    public EnvironmentApiKeyValidator(string environmentVariableName)
    {
        if (string.IsNullOrWhiteSpace(environmentVariableName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(environmentVariableName));
        _environmentVariableName = environmentVariableName;
    }
    
    public Task<Result> ValidateAsync(string apiKey)
    {
        var environmentApiKey = Environment.GetEnvironmentVariable(_environmentVariableName);
        if (string.IsNullOrWhiteSpace(environmentApiKey)) 
            return Task.FromResult(Result.Fail("ApiKey environment variable not set."));
        return apiKey == environmentApiKey
            ? Task.FromResult(Result.Ok())
            : Task.FromResult(Result.Fail("ApiKey is invalid"));
    }
}
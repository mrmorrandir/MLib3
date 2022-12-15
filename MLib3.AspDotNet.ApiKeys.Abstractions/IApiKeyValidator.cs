using FluentResults;

namespace MLib3.AspDotNet.ApiKeys.Abstractions;

public interface IApiKeyValidator
{
    Task<Result> ValidateAsync(string apiKey);
}
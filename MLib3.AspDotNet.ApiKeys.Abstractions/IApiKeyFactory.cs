namespace MLib3.AspDotNet.ApiKeys.Abstractions;

public interface IApiKeyFactory
{
    string Create(string prefix = "CT-", int length = 36);
}
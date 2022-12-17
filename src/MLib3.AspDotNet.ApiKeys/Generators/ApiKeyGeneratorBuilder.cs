using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys.Generators;

public class ApiKeyGeneratorBuilder
{
    private int Length { get; set; } = 32;
    private string Prefix { get; set; } = "ApiKey-";
    
    private static ApiKeyGeneratorBuilder CreateBuilder()
    {
        return new ApiKeyGeneratorBuilder();
    }

    public ApiKeyGeneratorBuilder WithLength(int length)
    {
        Length = length;
        return this;
    }
    
    public ApiKeyGeneratorBuilder WithPrefix(string prefix)
    {
        Prefix = prefix;
        return this;
    }

    public IApiKeyGenerator Build()
    {
        return new ApiKeyGenerator(Prefix, Length);
    }
}
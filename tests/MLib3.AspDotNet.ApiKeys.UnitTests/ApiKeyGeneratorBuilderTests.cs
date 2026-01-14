using MLib3.AspDotNet.ApiKeys.Generators;
using Xunit;

namespace MLib3.AspDotNet.ApiKeys.UnitTests;

public class ApiKeyGeneratorBuilderTests
{
    [Fact]
    public void ShouldBuild_WhenUsedWithoutOptions()
    {
        var apiKeyGeneratorBuilder = new ApiKeyGeneratorBuilder();
        
        var build = () => apiKeyGeneratorBuilder.Build();
        var apiKeyGenerator = build.Should().NotThrow().Subject;
    }
    
    [Fact]
    public void ShouldBuild_WhenUsedWithOptions()
    {
        var apiKeyGeneratorBuilder = new ApiKeyGeneratorBuilder();
        
        var build = () => apiKeyGeneratorBuilder
            .WithLength(32)
            .WithPrefix("ApiKey-")
            .Build();
        var apiKeyGenerator = build.Should().NotThrow().Subject;
    }
    
}
using System;
using FluentAssertions;
using MLib3.AspDotNet.ApiKeys.Generators;
using Xunit;

namespace MLib3.AspDotNet.ApiKeys.UnitTests;

public class ApiKeyGeneratorTests
{
    [Fact]
    public void ShouldGenerateValidApiKey_WhenCalled()
    {
        var apiKeyGenerator = new ApiKeyGenerator();
        
        var apiKey = apiKeyGenerator.Generate();

        apiKey.Should().StartWith("ApiKey-");
        apiKey.Should().HaveLength("ApiKey-".Length + 32);
    }
    
    [Fact]
    public void ShouldGenerateUniqueApiKeys_WhenCalledMultipleTimes()
    {
        var apiKeyGenerator = new ApiKeyGenerator();
        
        var apiKey1 = apiKeyGenerator.Generate();
        var apiKey2 = apiKeyGenerator.Generate();

        apiKey1.Should().NotBe(apiKey2);
    }
    
    [Fact]
    public void ShouldThrowArgumentException_WhenCreatedWithLengthIsLessThan32()
    {
        var init = () => new ApiKeyGenerator(length:31);
        
        init.Should().Throw<ArgumentException>();
    }
    
    
}
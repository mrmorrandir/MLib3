using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MLib3.AspDotNet.ApiKeys.Abstractions;
using MLib3.AspDotNet.ApiKeys.Generators;
using MLib3.AspDotNet.ApiKeys.Validators;
using Xunit;

namespace MLib3.AspDotNet.ApiKeys.UnitTests;

public class DependencyInjectionTests
{
    [Fact]
    public void ShouldRegisterStringApiKeyValidator_WhenUseApiKeyConfigurationIsUsed()
    {
        var services = new ServiceCollection();
        services.AddApiKeys(builder => builder.UseApiKey("test"));

        var serviceProvider = services.BuildServiceProvider();
        var apiKeyValidator = serviceProvider.GetRequiredService<IApiKeyValidator>();
        
        apiKeyValidator.Should().BeOfType<StringApiKeyValidator>();
    }
    
    [Fact]
    public void ShouldRegisterEnvironmentApiKeyValidator_WhenUseEnvironmentApiKeyIsUsed()
    {
        var services = new ServiceCollection();
        services.AddApiKeys(builder => builder.UseEnvironmentApiKey("HOST_API_KEY"));

        var serviceProvider = services.BuildServiceProvider();
        var apiKeyValidator = serviceProvider.GetRequiredService<IApiKeyValidator>();
        
        apiKeyValidator.Should().BeOfType<EnvironmentApiKeyValidator>();
    }
 
    [Fact]
    public void ShouldRegisterApiKeyGenerator_WhenUseApiKeyGeneratorIsUsed()
    {
        var services = new ServiceCollection();
        services.AddApiKeys(builder => builder.UseApiKeyGenerator(optionsBuilder => {}));

        var serviceProvider = services.BuildServiceProvider();
        var apiKeyGenerator = serviceProvider.GetRequiredService<IApiKeyGenerator>();
        
        apiKeyGenerator.Should().BeOfType<ApiKeyGenerator>();
    }
    
    [Fact]
    public void ShouldRegisterApiKeyGenerator_WhenUseApiKeyGeneratorIsUsedWithOptions()
    {
        var services = new ServiceCollection();
        services.AddApiKeys(builder => builder.UseApiKeyGenerator(options => options.WithPrefix("Test_").WithLength(50)));

        var serviceProvider = services.BuildServiceProvider();
        var apiKeyGenerator = serviceProvider.GetRequiredService<IApiKeyGenerator>();
        
        apiKeyGenerator.Should().BeOfType<ApiKeyGenerator>();
        var apiKey = apiKeyGenerator.Generate();
        apiKey.Should().StartWith("Test_");
        apiKey.Length.Should().Be("Test_".Length + 50);
    }
}
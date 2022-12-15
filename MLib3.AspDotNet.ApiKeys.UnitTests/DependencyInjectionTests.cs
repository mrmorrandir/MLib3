using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MLib3.AspDotNet.ApiKeys.Abstractions;
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
}
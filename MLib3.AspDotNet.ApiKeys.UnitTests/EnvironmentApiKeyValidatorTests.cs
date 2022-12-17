using System;
using System.Threading.Tasks;
using FluentAssertions;
using MLib3.AspDotNet.ApiKeys.Validators;
using Xunit;

namespace MLib3.AspDotNet.ApiKeys.UnitTests;

public class EnvironmentApiKeyValidatorTests
{
    [Fact]
    public async Task ShouldSucceed_WhenCalledWithValidApiKey()
    {
        Environment.SetEnvironmentVariable("API_KEY_TEST", "validApiKey");
        var validator = new EnvironmentApiKeyValidator("API_KEY_TEST");
        
        var result = await validator.ValidateAsync("validApiKey");

        result.IsSuccess.Should().BeTrue();
        Environment.SetEnvironmentVariable("API_KEY_TEST", null);
    }
    
    [Fact]
    public async Task ShouldFail_WhenCalledWithInvalidApiKey()
    {
        Environment.SetEnvironmentVariable("API_KEY_TEST", "validApiKey");
        var validator = new EnvironmentApiKeyValidator("API_KEY_TEST");
        
        var result = await validator.ValidateAsync("invalidApiKey");

        result.IsSuccess.Should().BeFalse();
        Environment.SetEnvironmentVariable("API_KEY_TEST", null);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldFail_WhenCalledWithNullOrWhiteSpaceApiKey(string apiKey)
    {
        Environment.SetEnvironmentVariable("API_KEY_TEST", "validApiKey");
        var validator = new EnvironmentApiKeyValidator("API_KEY_TEST");
        
        var result = await validator.ValidateAsync(apiKey);

        result.IsSuccess.Should().BeFalse();
        Environment.SetEnvironmentVariable("API_KEY_TEST", null);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldFail_WhenCalledWithEnvironmentVariableValueIsNullOrWhitespace(string? environmentVariableValue)
    {
        Environment.SetEnvironmentVariable("API_KEY_TEST", environmentVariableValue);
        
        var validator = new EnvironmentApiKeyValidator("API_KEY_TEST");

        var result= await validator.ValidateAsync("validApiKey");
        result.IsFailed.Should().BeTrue();
        
        Environment.SetEnvironmentVariable("API_KEY_TEST", null);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowArgumentException_WhenCreatedWithNullOrWhitespaceEnvironmentVariableName(string? environmentVariableName)
    {
        var func = () => new EnvironmentApiKeyValidator(environmentVariableName);
        
        func.Should().Throw<ArgumentException>().WithMessage("*Value*null*");
    }
    
   
}
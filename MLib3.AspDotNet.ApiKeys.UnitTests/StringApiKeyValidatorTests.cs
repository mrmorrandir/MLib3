using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MLib3.AspDotNet.ApiKeys.UnitTests;

public class StringApiKeyValidatorTests
{
    [Fact]
    public async Task ShouldSucceed_WhenCalledWithValidApiKey()
    {
        var validator = new StringApiKeyValidator("validApiKey");
        
        var result = await validator.ValidateAsync("validApiKey");

        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldFail_WhenCalledWithInvalidApiKey()
    {
        var validator = new StringApiKeyValidator("validApiKey");
        
        var result = await validator.ValidateAsync("invalidApiKey");

        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ShouldFail_WhenCalledWithNullOrWhiteSpaceApiKey(string apiKey)
    {
        var validator = new StringApiKeyValidator("validApiKey");
        
        var result = await validator.ValidateAsync(apiKey);

        result.IsSuccess.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowException_WhenCreatedWithNullOrWhitespaceApiKey(string? apiKey)
    {
        var func = () => new StringApiKeyValidator(apiKey);

        func.Should().Throw<ArgumentException>().WithParameterName("apiKey");
    }
}
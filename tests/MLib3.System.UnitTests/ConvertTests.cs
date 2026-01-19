using System;

// ReSharper disable once CheckNamespace
namespace MLib3.System.UnitTests;

public class ConvertTests
{
    [Fact]
    public void ToBase32_ShouldReturnEmpty_WhenDataEmpty()
    {
        // Arrange
        var bytes = Array.Empty<byte>();
        
        // Act
        var result = bytes.ToBase32String();

        // Assert

        result.Should().BeEmpty();
    }

    [Fact]
    public void ToBase32_ShouldEncodeCorrectly_WhenGivenData()
    {
        // Arrange

        var data = new byte[] { 0x66, 0x6f, 0x6f }; // "foo"

        // Act
        var result = data.ToBase32String();

        // Assert

        result.Should().Be("MZXW6");
    }
}

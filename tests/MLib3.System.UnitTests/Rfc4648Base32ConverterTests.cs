using System;

// ReSharper disable once CheckNamespace
namespace MLib3.System.UnitTests;

public class Rfc4648Base32ConverterTests
{
    [Fact]
    public void TryEncode_ShouldThrow_WhenDestinationTooSmall()
    {
        // Arrange

        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 1, 2, 3, 4 };
        var dest = new char[1];

        // Act

        Action action = () => converter.TryEncode(data, dest);

        // Assert

        action.Should().Throw<ArgumentException>();
    }
}

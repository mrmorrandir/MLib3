using System;

namespace MLib3.System.UnitTests;

public class Rfc4648Base32ConverterTests
{
    [Fact]
    public void Encode_ShouldReturnEmptyString_WhenDataIsEmpty()
    {
        var converter = new Rfc4648Base32Converter();
        var data = Array.Empty<byte>();

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public void Encode_ShouldEncodeCorrectly_WhenGivenSingleByte()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66 };

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("MY");
    }

    [Fact]
    public void Encode_ShouldEncodeCorrectly_WhenGivenMultipleBytes()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("MZXW6");
    }

    [Theory]
    [InlineData(new byte[] { 0x66 }, "MY")]
    [InlineData(new byte[] { 0x66, 0x6f }, "MZXQ")]
    [InlineData(new byte[] { 0x66, 0x6f, 0x6f }, "MZXW6")]
    [InlineData(new byte[] { 0x66, 0x6f, 0x6f, 0x62 }, "MZXW6YQ")]
    [InlineData(new byte[] { 0x66, 0x6f, 0x6f, 0x62, 0x61 }, "MZXW6YTB")]
    [InlineData(new byte[] { 0x66, 0x6f, 0x6f, 0x62, 0x61, 0x72 }, "MZXW6YTBOI")]
    public void Encode_ShouldEncodeCorrectly_WhenGivenVariousLengths(byte[] data, string expected)
    {
        var converter = new Rfc4648Base32Converter();

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public void Encode_ShouldReturnUppercase_WhenUppercaseIsTrue()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };

        var result = converter.Encode(data, uppercase: true);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("MZXW6");
    }

    [Fact]
    public void Encode_ShouldReturnLowercase_WhenUppercaseIsFalse()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };

        var result = converter.Encode(data, uppercase: false);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("mzxw6");
    }

    [Fact]
    public void Encode_ShouldEncodeAllBytesCorrectly_WhenGivenFullByteRange()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x00, 0xFF, 0x88, 0x12, 0x34 };

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("AD7YQERU");
    }

    [Fact]
    public void Encode_ShouldNotIncludePadding_WhenEncodingAnyData()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66 };

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotContain("=");
    }

    [Fact]
    public void TryEncode_ShouldReturnZero_WhenDataIsEmpty()
    {
        var converter = new Rfc4648Base32Converter();
        var data = Array.Empty<byte>();
        var destination = new char[10];

        var result = converter.TryEncode(data, destination);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(0);
    }

    [Fact]
    public void TryEncode_ShouldEncodeCorrectly_WhenDestinationIsLargeEnough()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };
        var destination = new char[10];

        var result = converter.TryEncode(data, destination);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(5);
        new string(destination, 0, result.Value).Should().Be("MZXW6");
    }

    [Fact]
    public void TryEncode_ShouldFail_WhenDestinationIsTooSmall()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };
        var destination = new char[4];

        var result = converter.TryEncode(data, destination);

        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Destination buffer is too small.");
    }

    [Fact]
    public void TryEncode_ShouldIncludeMetadata_WhenDestinationIsTooSmall()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };
        var destination = new char[2];

        var result = converter.TryEncode(data, destination);

        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["Required"].Should().Be(5);
        result.Errors[0].Metadata["Available"].Should().Be(2);
    }

    [Fact]
    public void TryEncode_ShouldEncodeInUppercase_WhenUppercaseIsTrue()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };
        var destination = new char[10];

        var result = converter.TryEncode(data, destination, uppercase: true);

        result.IsSuccess.Should().BeTrue();
        new string(destination, 0, result.Value).Should().Be("MZXW6");
    }

    [Fact]
    public void TryEncode_ShouldEncodeInLowercase_WhenUppercaseIsFalse()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };
        var destination = new char[10];

        var result = converter.TryEncode(data, destination, uppercase: false);

        result.IsSuccess.Should().BeTrue();
        new string(destination, 0, result.Value).Should().Be("mzxw6");
    }

    [Fact]
    public void TryEncode_ShouldWorkWithExactSizeBuffer_WhenDestinationIsExactlyNeeded()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x66, 0x6f, 0x6f };
        var destination = new char[5];

        var result = converter.TryEncode(data, destination);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(5);
        new string(destination).Should().Be("MZXW6");
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 4)]
    [InlineData(3, 5)]
    [InlineData(4, 7)]
    [InlineData(5, 8)]
    [InlineData(10, 16)]
    [InlineData(20, 32)]
    public void TryEncode_ShouldCalculateCorrectLength_WhenGivenVariousInputSizes(int inputSize, int expectedOutputSize)
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[inputSize];
        var destination = new char[expectedOutputSize];

        var result = converter.TryEncode(data, destination);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedOutputSize);
    }

    [Fact]
    public void TryEncode_ShouldHandleLargeData_WhenGivenManyBytes()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[1000];
        for (var i = 0; i < data.Length; i++)
            data[i] = (byte)(i % 256);
        var destination = new char[1600];

        var result = converter.TryEncode(data, destination);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Encode_ShouldHandleAllZeros_WhenGivenZeroBytes()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x00, 0x00, 0x00 };

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("AAAAA");
    }

    [Fact]
    public void Encode_ShouldHandleAllOnes_WhenGivenMaxBytes()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0xFF, 0xFF, 0xFF };

        var result = converter.Encode(data);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("77776");
    }

    [Fact]
    public void Encode_ShouldUseStandardAlphabet_WhenEncodingUppercase()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x00, 0x44, 0x32, 0x14, 0xC7, 0x42, 0x54, 0xB6, 0x35, 0xCF };

        var result = converter.Encode(data, uppercase: true);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().MatchRegex("^[A-Z2-7]+$");
    }

    [Fact]
    public void Encode_ShouldUseStandardAlphabet_WhenEncodingLowercase()
    {
        var converter = new Rfc4648Base32Converter();
        var data = new byte[] { 0x00, 0x44, 0x32, 0x14, 0xC7, 0x42, 0x54, 0xB6, 0x35, 0xCF };

        var result = converter.Encode(data, uppercase: false);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().MatchRegex("^[a-z2-7]+$");
    }
}

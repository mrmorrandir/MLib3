namespace MLib3.System;

/// <summary>
/// Implements RFC 4648 Base32 encoding without padding.
/// Uses the standard Base32 alphabet: A-Z and 2-7 for uppercase, or a-z and 2-7 for lowercase.
/// This implementation does not include '=' padding characters in the encoded output.
/// </summary>
public sealed class Rfc4648Base32Converter : IBase32Converter
{
    private const string _upperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
    private const string _lowerAlphabet = "abcdefghijklmnopqrstuvwxyz234567";

    /// <summary>
    /// Encodes binary data to a Base32 string without padding.
    /// </summary>
    /// <param name="data">The binary data to encode.</param>
    /// <param name="uppercase">When true, uses uppercase alphabet (A-Z, 2-7); when false, uses lowercase alphabet (a-z, 2-7). Default is true.</param>
    /// <returns>A Result containing the Base32-encoded string, or an error if encoding fails.</returns>
    public Result<string> Encode(ReadOnlySpan<byte> data, bool uppercase = true)
    {
        if (data.Length == 0)
            return Result.Ok(string.Empty);

        var lengthResult = GetEncodedLength(data.Length);
        if (lengthResult.IsFailed)
            return lengthResult.ToResult();

        var needed = lengthResult.Value;
        var buffer = new char[needed];

        var encodeResult = TryEncode(data, buffer, uppercase);
        if (encodeResult.IsFailed)
            return encodeResult.ToResult();

        return Result.Ok(new string(buffer, 0, encodeResult.Value));
    }

    /// <summary>
    /// Encodes binary data to Base32 directly into the provided destination buffer without allocating new strings.
    /// This zero-allocation variant is suitable for performance-critical scenarios.
    /// </summary>
    /// <param name="data">The binary data to encode.</param>
    /// <param name="destination">The destination buffer to write the encoded characters into.</param>
    /// <param name="uppercase">When true, uses uppercase alphabet (A-Z, 2-7); when false, uses lowercase alphabet (a-z, 2-7). Default is true.</param>
    /// <returns>A Result containing the number of characters written to the destination buffer, or an error if the buffer is too small or encoding fails.</returns>
    public Result<int> TryEncode(ReadOnlySpan<byte> data, Span<char> destination, bool uppercase = true)
    {
        if (data.Length == 0)
            return Result.Ok(0);

        var lengthResult = GetEncodedLength(data.Length);
        if (lengthResult.IsFailed)
            return lengthResult.ToResult();

        var needed = lengthResult.Value;
        if (destination.Length < needed)
        {
            return Result.Fail(new Error("Destination buffer is too small.")
                .WithMetadata("Required", needed)
                .WithMetadata("Available", destination.Length));
        }

        var alphabet = uppercase ? _upperAlphabet : _lowerAlphabet;

        var bitBuffer = 0;
        var bitsLeft = 0;
        var writeIndex = 0;

        for (var i = 0; i < data.Length; i++)
        {
            bitBuffer = ((bitBuffer << 8) | data[i]);
            bitsLeft += 8;

            while (bitsLeft >= 5)
            {
                var bitsToShift = (bitsLeft - 5);
                var index = ((bitBuffer >> bitsToShift) & 0x1F);
                bitsLeft -= 5;
                destination[writeIndex++] = alphabet[index];
            }
        }

        if (bitsLeft > 0)
        {
            var paddingShift = (5 - bitsLeft);
            var index = ((bitBuffer << paddingShift) & 0x1F);
            destination[writeIndex++] = alphabet[index];
        }

        return Result.Ok(writeIndex);
    }

    /// <summary>
    /// Calculates the required length of the Base32-encoded output for the given input byte count.
    /// </summary>
    /// <param name="inputByteCount">The number of input bytes to encode.</param>
    /// <returns>A Result containing the required character count for the encoded output, or an error if the input byte count is negative.</returns>
    private Result<int> GetEncodedLength(int inputByteCount)
    {
        if (inputByteCount < 0)
        {
            return Result.Fail(new Error("Input byte count cannot be negative.")
                .WithMetadata("InputByteCount", inputByteCount));
        }

        return Result.Ok((int)Math.Ceiling((inputByteCount * 8.0) / 5.0));
    }
}
namespace MLib3.System;

/// <summary>
/// Defines Base32 encoding operations according to RFC 4648 without padding.
/// Provides both allocating and zero-allocation encoding variants.
/// </summary>
public interface IBase32Converter
{
    /// <summary>
    /// Encodes binary data to a Base32 string without padding.
    /// </summary>
    /// <param name="data">The binary data to encode.</param>
    /// <param name="uppercase">When true, uses uppercase alphabet (A-Z, 2-7); when false, uses lowercase alphabet (a-z, 2-7). Default is true.</param>
    /// <returns>A Result containing the Base32-encoded string, or an error if encoding fails.</returns>
    Result<string> Encode(ReadOnlySpan<byte> data, bool uppercase = true);

    /// <summary>
    /// Encodes binary data to Base32 directly into the provided destination buffer without allocating new strings.
    /// This zero-allocation variant is suitable for performance-critical scenarios.
    /// </summary>
    /// <param name="data">The binary data to encode.</param>
    /// <param name="destination">The destination buffer to write the encoded characters into.</param>
    /// <param name="uppercase">When true, uses uppercase alphabet (A-Z, 2-7); when false, uses lowercase alphabet (a-z, 2-7). Default is true.</param>
    /// <returns>A Result containing the number of characters written to the destination buffer, or an error if the buffer is too small or encoding fails.</returns>
    Result<int> TryEncode(ReadOnlySpan<byte> data, Span<char> destination, bool uppercase = true);
}
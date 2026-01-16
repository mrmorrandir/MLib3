namespace MLib3.System;

public static class Convert
{
    /// <summary>
    /// Converts a byte array to its Base32-encoded string representation.
    /// </summary>
    /// <param name="data">The byte array to encode.</param>
    /// <returns>A Base32-encoded string representing the input byte array.</returns>
    /// <remarks>
    /// This method uses the RFC 4648 Base32 encoding scheme.
    /// </remarks>
    public static string ToBase32String(this byte[] data)
    {
        return ((ReadOnlySpan<byte>)data).ToBase32String();
    }

    /// <summary>
    /// Converts a byte array to its Base32-encoded string representation.
    /// </summary>
    /// <param name="data">The byte array to encode.</param>
    /// <returns>A Base32-encoded string representing the input byte array.</returns>
    /// <remarks>
    /// This method uses the RFC 4648 Base32 encoding scheme.
    /// </remarks>
    public static string ToBase32String(this ReadOnlySpan<byte> data)
    {
        var converter = new Rfc4648Base32Converter();
        var encodeResult = converter.Encode(data, true);
        return encodeResult.IsSuccess ? encodeResult.Value : throw new Exception(encodeResult.Errors[0].Message);
    }
}
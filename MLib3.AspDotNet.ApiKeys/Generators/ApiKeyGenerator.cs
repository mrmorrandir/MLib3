using System.Security.Cryptography;
using MLib3.AspDotNet.ApiKeys.Abstractions;

namespace MLib3.AspDotNet.ApiKeys.Generators;

public class ApiKeyGenerator : IApiKeyGenerator
{
    private readonly string _prefix;
    private readonly int _length;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyGenerator"/> class.
    /// </summary>
    /// <param name="prefix">The prefix identifies the type of ApiKey and is not counted to the length of the ApiKey that is generated</param>
    /// <param name="length">The length of the ApiKey (prefix not included)</param>
    /// <exception cref="ArgumentException">Throws an ArgumentException when the <paramref name="length"/> ist less than 32 characters.</exception>
    public ApiKeyGenerator(string prefix = "ApiKey-", int length = 32)
    {
        if (length < 32)
            throw new ArgumentException("Length must be at least 32 characters.", nameof(length));
        _prefix = prefix;
        _length = length;
    }
    
    /// <summary>
    ///   Generates a new API key.
    /// </summary>
    public string Generate() 
    {
        var bytes = RandomNumberGenerator.GetBytes(_length);
        var key = Convert.ToBase64String(bytes)
            .Replace("/", "")
            .Replace("+", "")
            .Replace("=", "")
            .AsSpan(0, _length);
    
        return $"{_prefix}{key}";
    }
}
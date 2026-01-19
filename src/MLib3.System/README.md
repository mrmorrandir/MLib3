MLib3.System
=============

A small utility package providing system-focused helpers for the MLib3 family of libraries. Its primary purpose is to offer a standards-compliant, zero-allocation friendly Base32 encoding implementation (RFC 4648 without padding) together with a convenient dependency injection registration helper.

Overview
--------

- Implements RFC 4648 Base32 encoding without padding.
- Includes both allocating and zero-allocation encoding APIs for performance-sensitive scenarios.
- Provides a simple DI extension to register the Base32 converter in Microsoft.Extensions.DependencyInjection.

Key features
------------

- RFC 4648 Base32 encoding (uppercase and lowercase alphabets).
- Zero-allocation encoding into a provided destination buffer (suitable for high-throughput paths).
- Small, focused public surface that is easy to audit and reuse across services.

Public API (high level)
-----------------------

- `IBase32Converter` — Interface defining the encoding operations, including an allocating `Encode` method returning a `Result<string>` and a zero-allocation `TryEncode` method returning a `Result<int>`.

- `Rfc4648Base32Converter` — Production implementation of `IBase32Converter` implementing RFC 4648 Base32 (no padding). This type implements both allocating and span-based zero-allocation encoders.

- `Convert` (extension) — Convenience extension methods to encode byte arrays or spans directly to Base32 strings using the default converter implementation.

- `DependencyInjection.AddBase32Converter` — Extension method to register the `IBase32Converter` implementation as a singleton with `IServiceCollection`.

using Microsoft.AspNetCore.Http.Metadata;

namespace MLib3.AspNetCore;

/// <summary>
/// Resolves the declared request and response DTO types associated with an endpoint.
/// These types allow the logging sanitizer to apply attributes without deserializing
/// or modifying the actual HTTP payloads.
/// </summary>
internal static class ApiLogPayloadTypeResolver
{
    /// <summary>
    /// Resolves request and response types from endpoint metadata.
    /// </summary>
    /// <param name="endpoint">The endpoint selected by ASP.NET Core routing.</param>
    /// <param name="statusCode">The actual HTTP response status code.</param>
    /// <returns>
    /// The resolved request and response types. Either value can be
    /// <see langword="null"/> when the endpoint does not provide reliable metadata.
    /// </returns>
    internal static (Type? RequestType, Type? ResponseType) Resolve(Endpoint? endpoint, int statusCode)
    {
        if (endpoint is null)
            return (null, null);

        // Explicit logging metadata has the highest priority. It covers endpoints
        // whose payload types cannot be determined reliably from standard metadata.
        var explicitMetadata = endpoint.Metadata
            .GetOrderedMetadata<LogPayloadTypesMetadata>();
        var explicitRequestType = explicitMetadata
            .Select(metadata => metadata.RequestType)
            .LastOrDefault(type => type is not null);
        var explicitResponseType = explicitMetadata
            .Select(metadata => metadata.ResponseType)
            .LastOrDefault(type => type is not null);

        // Minimal APIs normally expose their body parameter through IAcceptsMetadata.
        // Only use this standard contract; do not infer a request DTO from arbitrary
        // handler parameters because they may be services, route values, or headers.
        var requestType = explicitRequestType
                          ?? endpoint.Metadata.GetOrderedMetadata<IAcceptsMetadata>()
                              .Select(metadata => metadata.RequestType)
                              .FirstOrDefault(type => type is not null);

        // An endpoint can declare multiple response types for different status codes.
        // Ignore entries without a concrete DTO because they cannot drive attributes.
        var producesMetadata = endpoint.Metadata
            .GetOrderedMetadata<IProducesResponseTypeMetadata>()
            .Where(metadata => metadata.Type is not null)
            .ToArray();

        // Prefer explicit metadata, then the declaration matching the actual response
        // status, and finally the first successful response declaration as a fallback.
        var responseType = explicitResponseType
                           ?? producesMetadata.FirstOrDefault(metadata => metadata.StatusCode == statusCode)?.Type
                           ?? producesMetadata.FirstOrDefault(metadata =>
                               metadata.StatusCode is >= StatusCodes.Status200OK and < StatusCodes.Status300MultipleChoices)?.Type;

        return (requestType, responseType);
    }
}

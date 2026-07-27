namespace MLib3.AspNetCore;

/// <summary>
/// Adds explicit logging payload type metadata to endpoints.
/// </summary>
public static class LogPayloadEndpointConventionExtensions
{
    /// <summary>
    /// Declares both DTO types used to sanitize the request and response logging payloads.
    /// Use this when standard endpoint metadata does not reliably expose either type.
    /// </summary>
    /// <typeparam name="TRequest">The request DTO type.</typeparam>
    /// <typeparam name="TResponse">The response DTO type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder WithLogPayloadTypes<TRequest, TResponse>(
        this IEndpointConventionBuilder builder)
    {
        builder.WithMetadata(new LogPayloadTypesMetadata(typeof(TRequest), typeof(TResponse)));
        return builder;
    }

    /// <summary>
    /// Declares the DTO type used to sanitize the request logging payload.
    /// Use this when standard endpoint metadata does not reliably expose the request type.
    /// </summary>
    /// <typeparam name="TRequest">The request DTO type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder WithLogPayloadRequestType<TRequest>(
        this IEndpointConventionBuilder builder)
    {
        builder.WithMetadata(new LogPayloadTypesMetadata(typeof(TRequest), null));
        return builder;
    }

    /// <summary>
    /// Declares the DTO type used to sanitize the response logging payload.
    /// Use this when standard endpoint metadata does not reliably expose the response type.
    /// </summary>
    /// <typeparam name="TResponse">The response DTO type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder WithLogPayloadResponseType<TResponse>(
        this IEndpointConventionBuilder builder)
    {
        builder.WithMetadata(new LogPayloadTypesMetadata(null, typeof(TResponse)));
        return builder;
    }
}

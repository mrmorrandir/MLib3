namespace MLib3.AspNetCore;

/// <summary>
/// Provides the contract for defining API endpoints within the application.
/// </summary>
/// <remarks>
/// Classes implementing this interface are responsible for registering specific
/// endpoints with the provided web application instance.
/// This allows defining customizable, extensible, and cohesive endpoints for
/// handling various functionality within the application, such as persisting data,
/// fetching resources, manipulating entities, or orchestrating business processes.
/// </remarks>
public interface IEndpoint
{
    /// <summary>
    /// Registers the specific implementation of the endpoint with the provided web application instance.
    /// </summary>
    /// <param name="app">The current instance of the web application to which the endpoint will be registered.</param>
    void Register(WebApplication app);
}
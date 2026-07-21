using Mediator;
using Microsoft.Extensions.Logging;

namespace MLib3.AspNetCore.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IMessage
    where TResponse : IResultBase
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(request, cancellationToken);
        if (response.IsFailed)
            _logger.LogWarning("Request: {{ {@Request} }} | Response: {{ {@Response} }}", request, response);
        else 
            _logger.LogInformation("Request: {{ {@Request} }} | Response: {{ {@Response} }}", request, response);
        return response;
    }
}
using Mediator;
using Microsoft.Extensions.Logging;

namespace MLib3.AspNetCore.Application.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : IResultBase
{
    private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for request {RequestType} with content: {@Request}", typeof(TRequest).Name, request);
            
            if (!typeof(TResponse).IsGenericType)
                return (TResponse)typeof(TResponse)
                    .GetMethod(nameof(Result.Fail), 0, [typeof(IError)])!
                    .Invoke(null, [new ExceptionalError(ex)])!;
            
            var genericType = typeof(TResponse).GetGenericArguments()[0];
            // Get the MethodInfo for the generic Result.Fail<> method
            var failMethod = typeof(Result)
                .GetMethod(nameof(Result.Fail), 1, [typeof(IError)])!
                .MakeGenericMethod(genericType);
            return (TResponse)failMethod.Invoke(null, [new ExceptionalError(ex)])!;
        }
    }
}
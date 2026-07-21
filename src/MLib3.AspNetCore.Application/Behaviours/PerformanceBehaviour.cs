using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace MLib3.AspNetCore.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IMessage
{
    private readonly ILogger _logger;
    private readonly int _millisecondsWarningThreshold;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger, int millisecondsWarningThreshold = 1000)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _millisecondsWarningThreshold = millisecondsWarningThreshold;
    }
    
    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next(request, cancellationToken);
        _timer.Stop();

        var requestName = typeof(TRequest).Name;
        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds <= _millisecondsWarningThreshold)
        {
            _logger.LogDebug("Request: {Name} ({ElapsedMilliseconds}ms) {@Request}", requestName, elapsedMilliseconds, request);
            return response;
        }

        _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds}ms) {@Request}", requestName, elapsedMilliseconds, request);
        return response;
    }
}
using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;
using MLib3.Logging;

namespace MLib3.AspNetCore.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : IResultBase
{
    private readonly ILogger _logger;
    private readonly ILogPayloadSanitizer _payloadSanitizer;

    public LoggingBehaviour(
        ILogger<LoggingBehaviour<TRequest, TResponse>> logger,
        ILogPayloadSanitizer payloadSanitizer)
    {
        _logger = logger;
        _payloadSanitizer = payloadSanitizer;
    }

    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next(request, cancellationToken);
        stopwatch.Stop();

        var requestPayload = _payloadSanitizer.Sanitize(request, typeof(TRequest));
        var responsePayload = _payloadSanitizer.Sanitize(response, typeof(TResponse));
        if (!requestPayload.IsSuccess || !responsePayload.IsSuccess)
        {
            _logger.LogWarning(
                "Logging payload sanitization failed for request type {RequestType}; affected payloads were omitted",
                typeof(TRequest).Name);
        }

        var requestJson = GetPayload(requestPayload);
        var responseJson = GetPayload(responsePayload);
        var resultStatus = response.IsFailed ? "Failed" : "Success";

        if (response.IsFailed)
        {
            _logger.LogWarning(
                "Request {RequestType} completed in {Duration}ms with result {ResultStatus} | Request: {RequestPayload} | Response: {ResponsePayload}",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                resultStatus,
                requestJson,
                responseJson);
        }
        else
        {
            _logger.LogInformation(
                "Request {RequestType} completed in {Duration}ms with result {ResultStatus} | Request: {RequestPayload} | Response: {ResponsePayload}",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                resultStatus,
                requestJson,
                responseJson);
        }

        return response;
    }

    private static string? GetPayload(LogPayloadSanitizationResult result)
    {
        return result.IsSuccess && !result.IsExcluded ? result.Payload : null;
    }
}

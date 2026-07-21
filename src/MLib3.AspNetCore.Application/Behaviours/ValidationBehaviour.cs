using FluentValidation;
using Mediator;

namespace MLib3.AspNetCore.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : IResultBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) 
            return await next(request, cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (!failures.Any()) return await next(request, cancellationToken);

        if (!typeof(TResponse).IsGenericType)
            return (TResponse)typeof(TResponse)
                .GetMethod(nameof(Result.Fail), 0, [typeof(IError)])!
                .Invoke(null, [new Error("Validation").CausedBy(failures.Select(f => f.ErrorMessage).ToArray())])!;

        var genericType = typeof(TResponse).GetGenericArguments()[0];
        // Get the MethodInfo for the generic Result.Fail<> method
        var failMethod = typeof(Result)
            .GetMethod(nameof(Result.Fail), 1, [typeof(IError)])!
            .MakeGenericMethod(genericType);
        return (TResponse)failMethod!.Invoke(null, [new Error("Validation").CausedBy(failures.Select(f => f.ErrorMessage).ToArray())])!;
    }
}
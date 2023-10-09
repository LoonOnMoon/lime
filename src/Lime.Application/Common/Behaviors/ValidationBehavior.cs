using FluentValidation;

using MediatR;

namespace Lime.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? validator;

    public ValidationBehavior(
        IValidator<TRequest>? validator = null)
    {
        this.validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (this.validator is null)
        {
            return await next();
        }

        var validationResult = await this.validator.ValidateAsync(request, options => options.ThrowOnFailures(), cancellationToken);

        return await next();
    }
}
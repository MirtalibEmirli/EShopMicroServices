using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>  : IPipelineBehavior<TRequest, TResponse>
    where TRequest:ICommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        ValidatorOptions.LanguageManager.Culture = new CultureInfo("az");
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.SelectMany(r=>r.Errors).Where(f=>f!=null).ToList();
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();    
    }
}

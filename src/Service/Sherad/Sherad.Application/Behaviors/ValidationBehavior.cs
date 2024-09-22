using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;


        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) 
            => _validators = validators;


        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators
                .Select(validator => validator
                .ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .Where(validator => !validator.IsValid)
                .Select(validator => validator.ToString("\n"))
                .ToList();

            if(errors.Any())
            {
                return (TResponse)(object)new BadRequestObjectResult(errors);
            }
            else
            {
                return await next();
            }
        }
    }
}

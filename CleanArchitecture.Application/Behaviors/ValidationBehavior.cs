﻿using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CleanArchitecture.Application.Behaviors;
//Validasyon kurallarımızıı kontrol edip dönecek hata mesajlarının ne olduğunu belirleyecek
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : class, IRequest<TResponse>

{

	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any())
		{
			return await next(); // Sıradaki middleware' e geçer hata yoksa
		}

		var context = new ValidationContext<TRequest>(request);

		var errorDictionary = _validators
				.Select(s => s.Validate(context))
				.SelectMany(s => s.Errors)
				.Where(s => s != null)
				.GroupBy(s => s.PropertyName, s => s.ErrorMessage, (propertyName, errorMessage) => new
				{
					Key = propertyName,
					Values = errorMessage.Distinct().ToArray()
				})
				.ToDictionary(s => s.Key, s => s.Values[0]);

		if (errorDictionary.Any())
		{
			var errors = errorDictionary.Select(s => new ValidationFailure
			{
				PropertyName = s.Value,
				ErrorCode = s.Key
			});
			throw new ValidationException(errors);
		}

		return await next();
	}
}
// Validasyon kurallarını kontrol ettirdik eğer bir hata varsa PropertName ve ErrorCode olarak ayırdık, dict içine aldık

// Daha sonrasında Hata varsa fırlattık
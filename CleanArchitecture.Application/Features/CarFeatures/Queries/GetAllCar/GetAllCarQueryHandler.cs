﻿using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCar
{
	public sealed class GetAllCarQueryHandler : IRequestHandler<GetAllCarQuery, IList<Car>>
	{
		private readonly ICarService _carService;

		public GetAllCarQueryHandler(ICarService carService)
		{
			_carService = carService;
		}

		public async Task<IList<Car>> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
		{
			IList<Car> cars = await _carService.GetAllAsync(request, cancellationToken);
			return cars;

		}
	}
}

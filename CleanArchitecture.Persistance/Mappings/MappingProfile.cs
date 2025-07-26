using AutoMapper;
using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Persistance.Mappings;

public sealed class MappingProfile : Profile //Profile sınıfı AutoMapper' dan gelir
{
	public MappingProfile()
	{
		CreateMap<CreateCarCommand, Car>().ReverseMap();
	}
}

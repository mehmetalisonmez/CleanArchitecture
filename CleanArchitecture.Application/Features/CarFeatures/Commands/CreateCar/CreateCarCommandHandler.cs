using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;

public sealed class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, MessageResponse> //IrequestHandler' da 1. olarak request'imiz 2. oalrak response' umuz
{
	private readonly ICarService _carService;

	public CreateCarCommandHandler(ICarService carService)
	{
		_carService = carService;
	}

	public async Task<MessageResponse> Handle(CreateCarCommand request, CancellationToken cancellationToken)
	{
		await _carService.CreateAsync(request, cancellationToken);
		return new("Araç başarıyla kaydedildi!");
	}
}

//Api' den isteği yaptık (create isteği) bu isteği application' a iletmek için CreateCarCommand kullanılır
//İstek yapıdığında handle response olarak ne döneceğini belirtiyor aradağı bağlantıyı hnadle sağlar
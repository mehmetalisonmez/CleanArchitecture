using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, MessageResponse>
{
	private readonly IAuthService _authService;
	private readonly IMailService _mailService;
	public RegisterCommandHandler(IAuthService authService, IMailService mailService)
	{
		_authService = authService;
		_mailService = mailService;
	}

	public async Task<MessageResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{

		await _mailService.SendMailAsync(request.Email, "Tebrikler başarılı bir kayıt oluşturdunuz");
		await _authService.RegisterAsync(request);			
		return new("Kullanıcı kaydı başarıyla tamamlandı");
	}
}






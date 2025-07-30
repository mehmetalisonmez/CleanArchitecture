using CleanArchitecture.Application.Services;
using FluentEmail.Core;

namespace CleanArchitecture.Infrastructure.Services;

public sealed class MailService : IMailService
{
	private readonly IFluentEmail _fluentEmail;

	public MailService(IFluentEmail fluentEmail)
	{
		_fluentEmail = fluentEmail;
	}

	public async Task SendMailAsync(string email, string body)
	{
		await _fluentEmail.To(email).Body(body).SendAsync();
	}
}


//Razor ve liquid template FluentEmail' de ne demek araştır!!
namespace CleanArchitecture.Application.Services;

public interface IMailService
{
	Task SendMailAsync(string email, string body);
}


namespace PetHome.Application.Interfaces;

public interface IEmailService
{
	Task SendEmailAsync(string to, string subject, string templateName, object model);
}

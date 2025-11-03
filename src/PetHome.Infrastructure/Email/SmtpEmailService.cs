using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Email;

public class SmtpEmailService : IEmailService
{
	private readonly IConfiguration _config;
	public SmtpEmailService(IConfiguration config)
	{
		_config = config;
	}

	public async Task SendEmailAsync(string to, string subject, string templateName, object model)
	{
		var htmlBody = await EmailTemplateRenderer.RenderAsync(templateName, model);
		// 2️⃣ Load SMTP settings from configuration
		var smtpSection = _config.GetSection("Smtp");
		var host = smtpSection["Host"];
		var port = int.Parse(smtpSection["Port"] ?? "587");
		var username = smtpSection["Username"];
		var password = smtpSection["Password"];
		var from = smtpSection["From"];
		var enableSsl = bool.Parse(smtpSection["EnableSsl"] ?? "true");

		// 3️⃣ Create the email message
		var mail = new MailMessage
		{
			From = new MailAddress(from),
			Subject = subject,
			Body = htmlBody,
			IsBodyHtml = true
		};
		mail.To.Add(to);

		// 4️⃣ Configure and send using SmtpClient
		using var smtp = new SmtpClient(host, port)
		{
			Credentials = new NetworkCredential(username, password),
			EnableSsl = enableSsl
		};

		await smtp.SendMailAsync(mail);
	}
}
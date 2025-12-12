using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Projeto1.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendTokenEmailAsync(string email, string token)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            var senderEmail = emailSettings["SenderEmail"];
            var senderPassword = emailSettings["SenderPassword"];
            var enableSsl = bool.Parse(emailSettings["EnableSsl"] ?? "true");

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail ?? "noreply@example.com"),
                Subject = "Token de Validação - Login",
                Body = $@"
                    <h2>Token de Validação</h2>
                    <p>Seu token de validação é: <strong>{token}</strong></p>
                    <p>Este token expira em 5 minutos.</p>
                    <p>Se você não solicitou este token, ignore este e-mail.</p>
                ",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Token enviado para {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar e-mail para {email}");
            throw;
        }
    }
}

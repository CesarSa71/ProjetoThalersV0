namespace Projeto1.Services;

public interface IEmailService
{
    Task SendTokenEmailAsync(string email, string token);
}
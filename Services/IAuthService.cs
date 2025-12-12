namespace Projeto1.Services;

public interface IAuthService
{
    Task<bool> UserExistsAsync(string email);
    Task<bool> ValidateUserTokenAsync(string email, string token);
    Task SendTokenToUserAsync(string email);
}
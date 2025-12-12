namespace Projeto1.Services;

public interface ITokenService
{
    string GenerateToken();
    void StoreToken(string email, string token);
    bool ValidateToken(string email, string token);
}

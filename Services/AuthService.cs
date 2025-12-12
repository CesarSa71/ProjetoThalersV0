using Microsoft.EntityFrameworkCore;
using Projeto1.Data;
using Projeto1.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Projeto1.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AuthService(
        ApplicationDbContext context,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _context = context;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ValidateUserTokenAsync(string email, string token)
    {
        // Verifica se o usuário existe
        var userExists = await UserExistsAsync(email);
        if (!userExists)
        {
            return false;
        }

        // Valida o token
        return _tokenService.ValidateToken(email, token);
    }

    public async Task SendTokenToUserAsync(string email)
    {
        var userExists = await UserExistsAsync(email);
        if (userExists)
        {
            var token = _tokenService.GenerateToken();
            _tokenService.StoreToken(email, token);
            await _emailService.SendTokenEmailAsync(email, token);
        }
    }
}
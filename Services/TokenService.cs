using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Projeto1.Services;

public class TokenService : ITokenService
{
    private readonly IMemoryCache _cache;
    private const int TOKEN_EXPIRATION_MINUTES = 5;

    public TokenService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public string GenerateToken()
    {
        // Gera um token de 6 dígitos
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public void StoreToken(string email, string token)
    {
        var cacheKey = $"token_{email}";
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(TOKEN_EXPIRATION_MINUTES),
            SlidingExpiration = TimeSpan.FromMinutes(TOKEN_EXPIRATION_MINUTES)
        };

        _cache.Set(cacheKey, token, cacheOptions);
    }

    public bool ValidateToken(string email, string token)
    {
        var cacheKey = $"token_{email}";
        
        if (_cache.TryGetValue(cacheKey, out string? storedToken))
        {
            if (storedToken == token)
            {
                // Remove o token após validação bem-sucedida
                _cache.Remove(cacheKey);
                return true;
            }
        }

        return false;
    }
}

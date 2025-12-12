using Projeto1.Models;

namespace Projeto1.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            return; // DB já foi inicializado
        }

        var users = new User[]
        {
            new User { Email = "teste@example.com", Name = "Usuário Teste" },
            new User { Email = "admin@example.com", Name = "Administrador" }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}

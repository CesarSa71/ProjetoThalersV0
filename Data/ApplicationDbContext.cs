using Microsoft.EntityFrameworkCore;
using Projeto1.Models;

namespace Projeto1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar para usar aspas nos nomes de tabelas e colunas (case-sensitive)
        modelBuilder.Entity<Pessoa>().ToTable("Pessoas");
        modelBuilder.Entity<User>().ToTable("Users");
    }
}

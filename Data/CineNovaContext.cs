using CineNova.Models;
using Microsoft.EntityFrameworkCore;

namespace CineNova.Data;

public class CineNovaContext : DbContext
{
    public CineNovaContext(DbContextOptions<CineNovaContext> options)
        : base(options)
    {
    }

    public DbSet<Pelicula> Peliculas => Set<Pelicula>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nombre)
            .IsUnique();

        modelBuilder.Entity<Pelicula>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Peliculas)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria
            {
                Id = 1,
                Nombre = "Acción",
                Descripcion = "Películas de acción y aventura",
                Estado = true
            },
            new Categoria
            {
                Id = 2,
                Nombre = "Comedia",
                Descripcion = "Películas de humor",
                Estado = true
            },
            new Categoria
            {
                Id = 3,
                Nombre = "Drama",
                Descripcion = "Historias dramáticas",
                Estado = true
            },
            new Categoria
            {
                Id = 4,
                Nombre = "Terror",
                Descripcion = "Suspenso y terror",
                Estado = true
            },
            new Categoria
            {
                Id = 5,
                Nombre = "Ciencia ficción",
                Descripcion = "Historias de ciencia ficción",
                Estado = true
            },
            new Categoria
            {
                Id = 6,
                Nombre = "Animación",
                Descripcion = "Películas animadas",
                Estado = true
            }
        );
    }
}

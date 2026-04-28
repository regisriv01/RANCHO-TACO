using Microsoft.EntityFrameworkCore;
using RANCHO_TACO.Models;
using RanchoTacoSQLite.Models;
namespace RanchoTaco.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Resena> Resenas { get; set; }

    }
}
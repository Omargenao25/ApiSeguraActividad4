using ApiSeguraActividad4.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSeguraActividad4.Data
{
    public class BibliotecaActualizadaDbContext : DbContext
    {
        public BibliotecaActualizadaDbContext(DbContextOptions<BibliotecaActualizadaDbContext> options) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Libro>()
              .HasOne(l => l.Autor)
              .WithMany(a => a.Libros)
              .HasForeignKey(l => l.AutorId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Libro>()
              .Property(l => l.Precio)
              .HasPrecision(18, 2);
        }
    }
}
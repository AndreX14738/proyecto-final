using Microsoft.EntityFrameworkCore;
using proyectodesarro.Models;

namespace proyectodesarro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Nota> Notas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales de las entidades
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Nombre)
                .IsUnique();

            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.DocumentoIdentidad)
                .IsUnique();

            // Relaciones
            modelBuilder.Entity<Asistencia>()
                .HasOne<Estudiante>()
                .WithMany()
                .HasForeignKey(a => a.EstudianteId);

            modelBuilder.Entity<Nota>()
                .HasOne<Estudiante>()
                .WithMany()
                .HasForeignKey(n => n.EstudianteId);
        }
    }
}

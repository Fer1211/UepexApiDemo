using Microsoft.EntityFrameworkCore;
using UepexApiDemo.Models;

namespace UepexApiDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }  // <-- NUEVO

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ======================
            // Configuración: Estudiante
            // ======================
            modelBuilder.Entity<Estudiante>(entity =>
            {
                // PK en NumeroDocumento
                entity.HasKey(e => e.NumeroDocumento);

                // Requeridos (NOT NULL)
                entity.Property(e => e.TipoDocumento).IsRequired();
                entity.Property(e => e.NombreEstudiante).IsRequired();
                entity.Property(e => e.ApellidosEstudiante).IsRequired();
                entity.Property(e => e.CodigoCurso).IsRequired();
                entity.Property(e => e.CondicionEstudiante).IsRequired();
                entity.Property(e => e.Curso).IsRequired();
                entity.Property(e => e.Regional).IsRequired();
                entity.Property(e => e.TipoCuenta).IsRequired();
                entity.Property(e => e.Moneda).IsRequired();
                entity.Property(e => e.Banco).IsRequired();
                entity.Property(e => e.CuentaBancariaEstudiante).IsRequired();
                entity.Property(e => e.NombreCuentaBancaria).IsRequired();
                entity.Property(e => e.AsistenciaFechaHoraEntrada).IsRequired();
                entity.Property(e => e.AsistenciaFechaHoraSalida).IsRequired();

                // Índice único (ya es PK, así que es redundante pero no rompe)
                entity.HasIndex(e => e.NumeroDocumento).IsUnique();
            });

            // ======================
            // Configuración: Usuario
            // ======================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id); // PK identity/int

                entity.Property(u => u.NombreUsuario)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.ClaveHash)
                      .IsRequired();

                entity.Property(u => u.Rol)
                      .IsRequired()
                      .HasMaxLength(30);

                // Queremos que no existan dos "admin", dos "sofia", etc.
                entity.HasIndex(u => u.NombreUsuario)
                      .IsUnique();
            });

            // ======================
            // Seed de un usuario inicial
            // ======================
            // IMPORTANTE:
            // ClaveHash debe ser un hash BCrypt de la clave real.
            // Ejemplo abajo asume que la clave original era "1234".
            // Cámbialo luego por seguridad.

            var hashAdmin = BCrypt.Net.BCrypt.HashPassword("admin1234");
            // Ese hash representa "admin1234" con BCrypt. Lo puedes cambiar.
        

            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                Id = 1,
                NombreUsuario = "admin",
                ClaveHash = hashAdmin,
                Rol = "Administrador"
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Back.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Alimento> Alimentos => Set<Alimento>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<MenuAlimento> MenuAlimentos => Set<MenuAlimento>();
        public DbSet<RegistroHabito> RegistroHabitos => Set<RegistroHabito>();
        public DbSet<TipoComida> TiposComida => Set<TipoComida>();
        public DbSet<RegistroAlimento> RegistroAlimentos => Set<RegistroAlimento>();
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<TipoAccion> TiposAccion => Set<TipoAccion>();
        public DbSet<TipoResultado> TiposResultado => Set<TipoResultado>();

        // Audit logs
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        //logger
        //public DbSet<AuditLog> AuditLogs { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar Rol
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            // Configurar Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.HasIndex(e => e.Username).IsUnique();

                // Relación con Rol (Many to One)
                entity.HasOne(e => e.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(e => e.RolId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación con Log (One to Many)
                entity.HasMany(e => e.Logs)
                    .WithOne(l => l.Usuario)
                    .HasForeignKey(l => l.UsuarioId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configurar Estudiante
            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Peso)
                    .HasColumnType("decimal(5,2)");
                entity.Property(e => e.Altura)
                    .HasColumnType("decimal(4,2)");
                entity.Property(e => e.FechaNacimiento)
.HasColumnType("timestamp with time zone");

                entity.Property(e => e.IMC)
                    .HasColumnType("decimal(5,2)")
                    .HasComputedColumnSql("\"Peso\" / (\"Altura\" * \"Altura\")", true);

                // Relación con RegistroHabito (One to Many)
                entity.HasMany(e => e.RegistroHabitos)
                    .WithOne(rh => rh.Estudiante)
                    .HasForeignKey(rh => rh.EstudianteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar Alimento
            modelBuilder.Entity<Alimento>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Proteinas)
                    .HasColumnType("decimal(5,2)");
                entity.Property(e => e.Carbohidratos)
                    .HasColumnType("decimal(5,2)");
                entity.Property(e => e.Grasas)
                    .HasColumnType("decimal(5,2)");

                // Relación con MenuAlimento (One to Many)
                entity.HasMany(e => e.MenuAlimentos)
                    .WithOne(ma => ma.Alimento)
                    .HasForeignKey(ma => ma.AlimentoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con RegistroAlimento (One to Many)
                entity.HasMany(e => e.RegistroAlimentos)
                    .WithOne(ra => ra.Alimento)
                    .HasForeignKey(ra => ra.AlimentoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar Menu
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .IsRequired();

                // Relación con MenuAlimento (One to Many)
                entity.HasMany(e => e.MenuAlimentos)
                    .WithOne(ma => ma.Menu)
                    .HasForeignKey(ma => ma.MenuId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar MenuAlimento (Tabla de unión)
            modelBuilder.Entity<MenuAlimento>(entity =>
            {
                // Relación con Menu (Many to One)
                entity.HasOne(e => e.Menu)
                    .WithMany(m => m.MenuAlimentos)
                    .HasForeignKey(e => e.MenuId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Alimento (Many to One)
                entity.HasOne(e => e.Alimento)
                    .WithMany(a => a.MenuAlimentos)
                    .HasForeignKey(e => e.AlimentoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar RegistroHabito
            modelBuilder.Entity<RegistroHabito>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .IsRequired();

                // Relación con Estudiante (Many to One)
                entity.HasOne(e => e.Estudiante)
                    .WithMany(est => est.RegistroHabitos)
                    .HasForeignKey(e => e.EstudianteId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con RegistroAlimento (One to Many)
                entity.HasMany(e => e.RegistroAlimentos)
                    .WithOne(ra => ra.RegistroHabito)
                    .HasForeignKey(ra => ra.RegistroHabitoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar TipoComida
            modelBuilder.Entity<TipoComida>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(e => e.Nombre).IsUnique();

                // Relación con RegistroAlimento (One to Many)
                entity.HasMany(e => e.RegistroAlimentos)
                    .WithOne(ra => ra.TipoComida)
                    .HasForeignKey(ra => ra.TipoComidaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar RegistroAlimento
            modelBuilder.Entity<RegistroAlimento>(entity =>
            {
                entity.Property(e => e.Cantidad)
                    .IsRequired(false);

                // Relación con RegistroHabito (Many to One)
                entity.HasOne(e => e.RegistroHabito)
                    .WithMany(rh => rh.RegistroAlimentos)
                    .HasForeignKey(e => e.RegistroHabitoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Alimento (Many to One)
                entity.HasOne(e => e.Alimento)
                    .WithMany(a => a.RegistroAlimentos)
                    .HasForeignKey(e => e.AlimentoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con TipoComida (Many to One)
                entity.HasOne(e => e.TipoComida)
                    .WithMany(tc => tc.RegistroAlimentos)
                    .HasForeignKey(e => e.TipoComidaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar TipoAccion
            modelBuilder.Entity<TipoAccion>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                // Relación con Log (One to Many)
                entity.HasMany<Log>()
                    .WithOne(l => l.TipoAccion)
                    .HasForeignKey(l => l.TipoAccionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar TipoResultado
            modelBuilder.Entity<TipoResultado>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                // Relación con Log (One to Many)
                entity.HasMany<Log>()
                    .WithOne(l => l.Resultado)
                    .HasForeignKey(l => l.ResultadoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar Log
            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Detalle)
                    .IsRequired();
                entity.Property(e => e.Rol)
                    .HasMaxLength(50);
                entity.Property(e => e.Entidad)
                    .HasMaxLength(50);
                entity.Property(e => e.IP)
                    .HasMaxLength(50);

                // Relación con Usuario (Many to One)
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Logs)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Relación con TipoAccion (Many to One)
                entity.HasOne(e => e.TipoAccion)
                    .WithMany(ta => ta.Logs)
                    .HasForeignKey(e => e.TipoAccionId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación con TipoResultado (Many to One)
                entity.HasOne(e => e.Resultado)
                    .WithMany(tr => tr.Logs)
                    .HasForeignKey(e => e.ResultadoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
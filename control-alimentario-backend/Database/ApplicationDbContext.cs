using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace control_alimentario_backend.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // TODO: Agregar DbSets de cada entidad para crear las tablas
        // public DbSet<Table> Tables => Set<Table>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TODO: Configurar las entidades aquí ()relaciones, llaves foraneas, etc)
        }
    }
}
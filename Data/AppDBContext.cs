using Microsoft.EntityFrameworkCore;
using SIPAME.Models;

namespace SIPAME.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
                
        }

        public DbSet<Pago> Pago { get; set; }
        public DbSet<Contratacion> contratacion { get; set; }
        public DbSet<Paquete> Paquete { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Estatus> Estatus { get; set; }
        public DbSet<Zona> Zona { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction; // Cambiamos a NoAction
            }
        }

    }
}

using Microsoft.EntityFrameworkCore;
using PedidosApi.Domain.Entities;

namespace PedidosApi.Infra.Data
{
    public class PedidosContext : DbContext
    {
        public PedidosContext(DbContextOptions<PedidosContext> options) : base(options)
        { }
        public DbSet<PedidoEntity>? Pedidos { get; set; }
        public DbSet<PedidoItemEntity>? ItemsPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;

        }
    }
}

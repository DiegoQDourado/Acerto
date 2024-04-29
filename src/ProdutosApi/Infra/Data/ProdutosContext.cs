using Microsoft.EntityFrameworkCore;
using ProdutosApi.Domain.Entities;

namespace ProdutosApi.Infra.Data
{
    public class ProdutosContext : DbContext
    {
        public ProdutosContext(DbContextOptions<ProdutosContext> options) : base(options)
        { }
        public DbSet<ProdutoEntity>? Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProdutosContext).Assembly);
        }
    }
}

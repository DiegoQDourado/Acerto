using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdutosApi.Domain.Entities;

namespace ProdutosApi.Infra.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<ProdutoEntity>
    {
        public void Configure(EntityTypeBuilder<ProdutoEntity> builder)
        {
            builder.HasKey(p => p.Id).HasName("id");
            builder.Property(p => p.Nome).HasMaxLength(100).IsRequired().HasColumnName("nome");
            builder.Property(p => p.Descricao).HasMaxLength(150).IsRequired().HasColumnName("descricao");
            builder.Property(p => p.Preco).HasPrecision(14, 2).IsRequired().HasColumnName("preco");
            builder.Property(p => p.QuantidadeEstoque).HasColumnName("quantidade_estoque");
            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");
            builder.ToTable("produtos");
        }
    }
}

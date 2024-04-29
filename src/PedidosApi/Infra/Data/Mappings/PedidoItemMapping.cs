using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedidosApi.Domain.Entities;

namespace PedidosApi.Infra.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItemEntity>
    {
        public void Configure(EntityTypeBuilder<PedidoItemEntity> builder)
        {
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.PedidoId).HasColumnName("pedido_id");
            builder.Property(p => p.ItemId).HasColumnName("item_id");
            builder.HasKey(p => new { p.PedidoId, p.ItemId });
            builder.Property(p => p.ItemNome).HasMaxLength(100).IsRequired().HasColumnName("item_nome");
            builder.Property(p => p.ItemPreco).HasPrecision(14, 2).IsRequired().HasColumnName("preco");
            builder.Property(p => p.ItemQuantidade).HasColumnName("quantidade");
            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");
            builder.HasOne(c => c.Pedido)
                .WithMany(c => c.PedidoItems);

            builder.ToTable("pedidos_items");
        }
    }
}


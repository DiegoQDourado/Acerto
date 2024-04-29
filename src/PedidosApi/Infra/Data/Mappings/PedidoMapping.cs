using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedidosApi.Domain.Entities;

namespace PedidosApi.Infra.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<PedidoEntity>
    {
        public void Configure(EntityTypeBuilder<PedidoEntity> builder)
        {
            builder.HasKey(p => p.Id).HasName("id");
            builder.Property(p => p.Codigo).HasColumnName("codigo");
            builder.Property(p => p.Situacao).HasMaxLength(100).HasColumnName("situacao");
            builder.Property(p => p.Descricao).HasMaxLength(150).IsRequired().HasColumnName("descricao");
            builder.Property(p => p.ValorTotal).HasPrecision(14, 2).IsRequired().HasColumnName("valor_total");
            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");
            builder.Property(p => p.Status).HasConversion<int>().HasColumnName("status");
            builder.HasMany(c => c.PedidoItems)
                .WithOne(c => c.Pedido)
                .HasForeignKey(c => c.PedidoId);

            builder.ToTable("pedidos");
        }
    }
}

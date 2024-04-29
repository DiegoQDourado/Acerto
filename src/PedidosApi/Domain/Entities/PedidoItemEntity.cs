using FluentValidation.Results;
using PedidosApi.Domain.Entities.Validators;

namespace PedidosApi.Domain.Entities
{
    public class PedidoItemEntity : BaseEntity
    {
        public Guid PedidoId { get; init; }
        public Guid ItemId { get; init; }
        public string ItemNome { get; init; } = string.Empty;
        public decimal ItemPreco { get; init; }
        public int ItemQuantidade { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; init; }

        public PedidoEntity Pedido { get; init; }

        protected override ValidationResult GetValidation() =>
             new PedidoItemValidator().Validate(this);

    }
}

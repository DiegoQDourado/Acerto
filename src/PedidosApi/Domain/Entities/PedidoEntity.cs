using FluentValidation.Results;
using PedidosApi.Domain.Entities.Validators;
using PedidosApi.Domain.Enums;

namespace PedidosApi.Domain.Entities
{
    public class PedidoEntity : BaseEntity
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.EmAnalise;
        public string? Situacao { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public List<PedidoItemEntity> PedidoItems { get; set; }

        public bool ValidateValorTotal()
        {
            var total = PedidoItems.Sum(p => p.ItemPreco);
            if (total != ValorTotal) return false;

            return true;
        }


        protected override ValidationResult GetValidation() =>
             new PedidoValidator().Validate(this);
    }
}

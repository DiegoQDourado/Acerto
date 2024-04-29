using System.ComponentModel.DataAnnotations;

namespace PedidosApi.Domain.Models
{
    public record Pedido
    {
        [Required]
        public int Codigo { get; set; }
        [Required]
        public string? Descricao { get; set; }
        [Required]
        public decimal ValorTotal { get; set; }

        public string? Situacao { get; set; }

        public List<PedidoItem> PedidoItems { get; set; }
    }
}

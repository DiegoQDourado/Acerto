using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Enums;

namespace PedidosApi.Domain.Models
{
    public record PedidoResponse
    {
        public Guid Id { get; init; }
        public int Codigo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.EmAnalise;
        public string? Situacao { get; set; }
        public List<PedidoItemResponse> PedidoItems { get; set; }
    }

    public class PedidoItemResponse
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public decimal Preco { get; init; }
        public int ItemQuantidade { get; init; }
    }
}

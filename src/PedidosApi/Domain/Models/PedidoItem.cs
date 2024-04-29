using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PedidosApi.Domain.Models
{
    public record PedidoItem
    {
        [Required]
        public Guid Id { get; init; }

        [JsonIgnore]
        public string? Nome { get; set; }

        [JsonIgnore]
        public decimal Preco { get; set; }

        [Required]
        public int Quantidade { get; init; }
    }
}

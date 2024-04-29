using System.ComponentModel.DataAnnotations;

namespace ProdutosApi.Domain.Models
{
    public record Produto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public int QuantidadeEstoque { get; set; }
    }
}

using FluentValidation.Results;
using ProdutosApi.Domain.Entities.Validators;

namespace ProdutosApi.Domain.Entities
{
    public class ProdutoEntity : BaseEntity
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }

        protected override ValidationResult GetValidation() =>
             new ProdutoValidator().Validate(this);
    }
}

using FluentValidation;

namespace ProdutosApi.Domain.Entities.Validators
{
    public class ProdutoValidator : AbstractValidator<ProdutoEntity>
    {
        public ProdutoValidator()
        {
            RuleFor(t => t.Nome)
                .NotEmpty()
                .WithMessage("Descrição não pode ser vazia.");

            RuleFor(t => t.Nome)
                .MaximumLength(100)
                .WithMessage("Descrição não pode ter mais de 100 caracteres.");

            RuleFor(t => t.Descricao)
                .NotEmpty()
                .WithMessage("Descrição não pode ser vazia.");

            RuleFor(t => t.Descricao)
                .MaximumLength(150)
                .WithMessage("Descrição não pode ter mais de 150 caracteres.");

            RuleFor(t => t.Preco)
            .GreaterThan(0)
            .WithMessage("Preço tem que ser maior que 0.");

            RuleFor(t => t.QuantidadeEstoque)
            .GreaterThan(0)
            .WithMessage("Quantidade Estoque tem que ser maior que 0.");


        }
    }
}

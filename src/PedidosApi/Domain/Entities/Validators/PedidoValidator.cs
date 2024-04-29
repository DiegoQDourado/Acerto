using FluentValidation;

namespace PedidosApi.Domain.Entities.Validators
{
    public class PedidoValidator : AbstractValidator<PedidoEntity>
    {
        public PedidoValidator()
        {
            RuleFor(t => t.Codigo)
            .GreaterThan(0)
            .WithMessage("Código tem que ser maior que 0.");

            RuleFor(t => t.Descricao)
                .NotEmpty()
                .WithMessage("Descrição não pode ser vazia.");

            RuleFor(t => t.Descricao)
                .MaximumLength(150)
                .WithMessage("Descrição não pode ter mais de 150 caracteres.");

            RuleFor(t => t.ValorTotal)
            .GreaterThan(0)
            .WithMessage("Valor Total tem que ser maior que 0.");

            RuleForEach(t => t.PedidoItems)
               .NotNull()
               .WithMessage("Pedido Items deve ter ser informado.")
               .SetValidator(new PedidoItemValidator());
        }
    }
}

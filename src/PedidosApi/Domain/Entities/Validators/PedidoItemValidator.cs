using FluentValidation;

namespace PedidosApi.Domain.Entities.Validators
{
    public class PedidoItemValidator : AbstractValidator<PedidoItemEntity>
    {
        public PedidoItemValidator()
        {
            RuleFor(t => t.ItemQuantidade)
            .GreaterThan(0)
            .WithMessage("Item Quantidade tem que ser maior que 0.");
        }
    }
}

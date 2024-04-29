using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Models;

namespace PedidosApi.Domain.Factories.Impl
{
    internal class PedidoItemFactory : IPedidoItemFactory
    {
        public PedidoItemEntity From(Guid pedidoId, PedidoItem pedidoItem) =>
            new()
            {
                PedidoId = pedidoId,
                ItemId = pedidoItem.Id,
                ItemNome = pedidoItem.Nome,
                ItemPreco = pedidoItem.Preco,
                ItemQuantidade = pedidoItem.Quantidade
            };

        public PedidoItemResponse From(PedidoItemEntity pedidoItem) =>
            new()
            {
                Id = pedidoItem.ItemId,
                Nome = pedidoItem.ItemNome,
                Preco = pedidoItem.ItemPreco,
                ItemQuantidade = pedidoItem.ItemQuantidade,
            };
    }
}

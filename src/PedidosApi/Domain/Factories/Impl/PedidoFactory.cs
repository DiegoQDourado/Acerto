using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Models;

namespace PedidosApi.Domain.Factories.Impl
{
    internal class PedidoFactory(IPedidoItemFactory pedidoItemFactory) : IPedidoFactory
    {
        private readonly IPedidoItemFactory _pedidoItemFactory = pedidoItemFactory;

        public PedidoEntity From(Pedido pedido)
        {
            PedidoEntity pedidoEntity = new()
            {
                Codigo = pedido.Codigo,
                Descricao = pedido.Descricao,
                ValorTotal = pedido.ValorTotal,
                PedidoItems = new List<PedidoItemEntity>(pedido.PedidoItems.Count)
            };
            foreach (var pedidoItem in pedido.PedidoItems)
            {
                pedidoEntity.PedidoItems.Add(_pedidoItemFactory.From(pedidoEntity.Id, pedidoItem));
            };

            return pedidoEntity;
        }

        public PedidoResponse From(PedidoEntity pedidoEntity)
        {
            PedidoResponse pedido = new()
            {
                Id = pedidoEntity.Id,
                Codigo = pedidoEntity.Codigo,
                Descricao = pedidoEntity.Descricao,
                ValorTotal = pedidoEntity.ValorTotal,
                Status = pedidoEntity.Status,
                Situacao = pedidoEntity.Situacao,
                PedidoItems = new List<PedidoItemResponse>(pedidoEntity.PedidoItems.Count)
            };
            foreach (var pedidoItem in pedidoEntity.PedidoItems)
            {
                pedido.PedidoItems.Add(_pedidoItemFactory.From(pedidoItem));
            };

            return pedido;
        }

    }
}

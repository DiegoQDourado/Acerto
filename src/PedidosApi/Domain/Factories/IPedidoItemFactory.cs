using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Models;

namespace PedidosApi.Domain.Factories
{
    public interface IPedidoItemFactory
    {
        PedidoItemEntity From(Guid pedidoId, PedidoItem pedidoItem);
        PedidoItemResponse From(PedidoItemEntity pedidoItem);
    }
}
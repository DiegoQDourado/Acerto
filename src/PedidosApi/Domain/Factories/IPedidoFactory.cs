using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Models;

namespace PedidosApi.Domain.Factories
{
    public interface IPedidoFactory
    {
        PedidoEntity From(Pedido pedido);
        PedidoResponse From(PedidoEntity pedidoEntity);
    }
}

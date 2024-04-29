using PedidosApi.Domain.Entities;

namespace PedidosApi.Infra.Data.Repositories
{
    public interface IPedidosRepository
    {
        Task AddAsync(PedidoEntity produto);
        Task DeleteAsync(PedidoEntity produto);
        Task<PedidoEntity?> GetByAsync(int codigo);
        Task UpdateAsync(PedidoEntity produto);
    }
}

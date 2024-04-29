using Microsoft.EntityFrameworkCore;
using PedidosApi.Domain.Entities;

namespace PedidosApi.Infra.Data.Repositories.Impl
{
    internal class PedidosRepository(PedidosContext context) : IPedidosRepository
    {
        private readonly PedidosContext _context = context;

        public async Task AddAsync(PedidoEntity pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<PedidoEntity?> GetByAsync(int codigo) =>
            await _context.Pedidos.AsNoTracking().Include(p => p.PedidoItems).FirstOrDefaultAsync(p => p.Codigo == codigo);

        public async Task UpdateAsync(PedidoEntity pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PedidoEntity pedido)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }
}

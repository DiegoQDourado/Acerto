using Microsoft.EntityFrameworkCore;
using ProdutosApi.Domain.Entities;

namespace ProdutosApi.Infra.Data.Repositories.Impl
{
    internal class ProdutosRepository(ProdutosContext context) : IProdutosRepository
    {
        private readonly ProdutosContext _context = context;

        public async Task AddAsync(ProdutoEntity produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdutoEntity?> GetByAsync(Guid id) =>
            await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task UpdateAsync(ProdutoEntity produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProdutoEntity produto)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}

using ProdutosApi.Domain.Entities;

namespace ProdutosApi.Infra.Data.Repositories
{
    public interface IProdutosRepository
    {
        Task AddAsync(ProdutoEntity produto);
        Task DeleteAsync(ProdutoEntity produto);
        Task<ProdutoEntity?> GetByAsync(Guid id);
        Task UpdateAsync(ProdutoEntity produto);
    }
}

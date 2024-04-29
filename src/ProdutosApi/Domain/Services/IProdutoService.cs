using ProdutosApi.Domain.Entities;
using ProdutosApi.Domain.Models;

namespace ProdutosApi.Domain.Services
{
    public interface IProdutoService
    {
        List<string> ErrorsMessage { get; }
        Task<(int StatusCode, ProdutoEntity?)> AddAsync(Produto value);
        Task<(int StatusCode, bool IsSuccess)> DeleteAsync(Guid id);
        Task<ProdutoEntity?> GetByAsync(Guid id);
        Task<(int StatusCode, bool IsSuccess, int Quantity)> GetQuantidadeByAsync(Guid id);
        Task<(int StatusCode, bool IsSuccess)> UpdateAsync(Guid id, Produto value);
    }
}

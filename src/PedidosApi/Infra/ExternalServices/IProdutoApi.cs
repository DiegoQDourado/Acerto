using PedidosApi.Infra.Models;
using Refit;

namespace PedidosApi.Infra.ExternalServices
{
    public interface IProdutoApi
    {
        [Get("/produtos/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ProdutoResponse> GetProdutoByAsync(Guid id);
    }
}

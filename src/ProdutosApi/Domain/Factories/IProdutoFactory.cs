using ProdutosApi.Domain.Entities;
using ProdutosApi.Domain.Models;

namespace ProdutosApi.Domain.Factories
{
    public interface IProdutoFactory
    {
        ProdutoEntity From(Produto produto);
    }
}

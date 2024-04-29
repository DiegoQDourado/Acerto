using ProdutosApi.Domain.Entities;
using ProdutosApi.Domain.Models;

namespace ProdutosApi.Domain.Factories.Impl
{
    internal class ProdutoFactory : IProdutoFactory
    {
        public ProdutoEntity From(Produto produto) =>
            new() { Nome = produto.Nome, Preco = produto.Preco, Descricao = produto.Descricao, QuantidadeEstoque = produto.QuantidadeEstoque };

    }
}

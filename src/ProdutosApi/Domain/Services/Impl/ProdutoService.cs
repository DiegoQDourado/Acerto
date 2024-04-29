using ProdutosApi.Domain.Entities;
using ProdutosApi.Domain.Factories;
using ProdutosApi.Domain.Models;
using ProdutosApi.Infra.Data.Repositories;

namespace ProdutosApi.Domain.Services.Impl
{
    internal class ProdutoService : IProdutoService
    {
        private readonly IProdutosRepository _repository;
        private readonly IProdutoFactory _factory;
        private readonly ILogger _logger;
        public List<string> _errorsMessage = new();
        public List<string> ErrorsMessage { get => _errorsMessage; }

        public ProdutoService(IProdutosRepository produtosRepository, IProdutoFactory factory, ILogger<ProdutoService> logger)
        {
            _repository = produtosRepository;
            _factory = factory;
            _logger = logger;
        }

        public async Task<(int StatusCode, ProdutoEntity?)> AddAsync(Produto value)
        {
            var produto = _factory.From(value);

            if (!produto.IsValid())
            {
                _errorsMessage.AddRange(produto.Validate());
                return (400, null);
            }

            try
            {
                await _repository.AddAsync(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Concat("Error: ", ex.Message), ex);
                return (500, null);
            }

            return (201, produto);
        }

        public async Task<(int StatusCode, bool IsSuccess)> DeleteAsync(Guid id)
        {
            try
            {
                var produtoDB = await _repository.GetByAsync(id);

                if (produtoDB is null)
                {
                    return (404, false);
                }

                await _repository.DeleteAsync(produtoDB);
            }
            catch (Exception)
            {
                return (500, false);
            }

            return (200, true);
        }

        public async Task<ProdutoEntity?> GetByAsync(Guid id) =>
            await _repository.GetByAsync(id);

        public async Task<(int StatusCode, bool IsSuccess, int Quantity)> GetQuantidadeByAsync(Guid id)
        {
            try
            {
                var produto = await _repository.GetByAsync(id);
                if (produto is null) return (404, false, 0);

                return (200, true, produto.QuantidadeEstoque);
            }
            catch (Exception)
            {
                return (500, false, 0);
            }


        }

        public async Task<(int StatusCode, bool IsSuccess)> UpdateAsync(Guid id, Produto value)
        {
            try
            {
                var produtoDB = await _repository.GetByAsync(id);

                if (produtoDB is null)
                {
                    return (404, false);
                }

                produtoDB.Nome = value.Nome;
                produtoDB.Descricao = value.Descricao;
                produtoDB.Preco = value.Preco;
                produtoDB.QuantidadeEstoque = value.QuantidadeEstoque;
                produtoDB.ModifiedAt = DateTime.UtcNow;

                if (!produtoDB.IsValid())
                {
                    _errorsMessage.AddRange(produtoDB.Validate());
                    return (400, false);
                }

                await _repository.UpdateAsync(produtoDB);

                return (200, true);

            }
            catch (Exception)
            {
                return (500, false);
            }
        }

    }
}

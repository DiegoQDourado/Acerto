using Microsoft.EntityFrameworkCore;
using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Enums;
using PedidosApi.Domain.Factories;
using PedidosApi.Domain.Models;
using PedidosApi.Infra.Configs;
using PedidosApi.Infra.Data.Repositories;
using PedidosApi.Infra.ExternalServices;
using Refit;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Net;

namespace PedidosApi.Domain.Services.Impl
{
    internal class PedidoService : IPedidoService
    {
        private readonly IPedidosRepository _repository;
        private readonly IPedidoFactory _factory;
        private readonly ILogger _logger;
        private readonly IProdutoApi _produtoApi;
        private readonly ProdutoApiConfig _produtoApiConfig;
        private ConcurrentQueue<string> _produtoApiValidation = new();
        private List<string> _errorsMessage = new();
        public List<string> ErrorsMessage { get => _errorsMessage; }


        public PedidoService(
            IPedidosRepository produtosRepository,
            IPedidoFactory factory,
            ILogger<PedidoService> logger,
            ProdutoApiConfig produtoApiConfig,
            IProdutoApi produtoApi)
        {
            _repository = produtosRepository;
            _factory = factory;
            _logger = logger;
            _produtoApiConfig = produtoApiConfig;
            _produtoApi = produtoApi;
        }

        public async Task<(int StatusCode, PedidoResponse?)> AddAsync(Pedido value)
        {
            try
            {
                var pedidoItems = new ConcurrentQueue<PedidoItem>();
                var cts = new CancellationTokenSource();
                var tasks = new List<Task>(value.PedidoItems.Count);
                using var semaphore = new SemaphoreSlim(_produtoApiConfig.ParallelCallCount);

                foreach (var pedidoItem in value.PedidoItems)
                {
                    tasks.Add(GetProdutoAsync(semaphore, pedidoItem.Id, pedidoItem.Quantidade, pedidoItems, cts));
                }

                await Task.WhenAll(tasks);

                if (!_produtoApiValidation.IsEmpty)
                {
                    _errorsMessage.AddRange(_produtoApiValidation);
                    return (400, null);
                }

                value.PedidoItems.Clear();
                value.PedidoItems.AddRange(pedidoItems);
                var pedido = _factory.From(value);

                if (!pedido.IsValid())
                {
                    _errorsMessage.AddRange(pedido.Validate());
                    return (400, null);
                }

                if (!pedido.ValidateValorTotal())
                {
                    var rejectMessage = "Valor Total diferente da soma dos preços para os produtos adcionados.";
                    pedido.Situacao = rejectMessage;
                    pedido.Status = StatusPedido.Recusado;
                    _errorsMessage.Add(rejectMessage);
                }

                await _repository.AddAsync(pedido);
                var pedidoResponse =  _factory.From(pedido);

                return (201, pedidoResponse);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(string.Concat("Error: ", ex.Message), ex);
                if (ex?.InnerException is DbException dbException && dbException.SqlState == "23505")
                {
                    _errorsMessage.Add("Código de pedido já existe.");
                    return (400, null);
                }
                return (500, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Concat("Error: ", ex.Message), ex);
                return (500, null);
            }
        }
        public async Task<(int StatusCode, bool IsSuccess)> DeleteAsync(int codigo)
        {
            try
            {
                var pedidoDB = await _repository.GetByAsync(codigo);

                if (pedidoDB is null)
                {
                    return (404, false);
                }

                await _repository.DeleteAsync(pedidoDB);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Concat("Error: ", ex.Message), ex);
                return (500, false);
            }

            return (200, true);
        }
        public async Task<PedidoResponse?> GetByAsync(int codigo)
        {
            var pedido = await _repository.GetByAsync(codigo);
            if (pedido is null) return null;

            return _factory.From(pedido);
        }
        public async Task<(int StatusCode, bool IsSuccess)> UpdateStatusAsync(int codigo, StatusPedido status, string motivo)
        {
            try
            {
                var pedidoDB = await _repository.GetByAsync(codigo);

                if (pedidoDB is null)
                {
                    return (404, false);
                }

                pedidoDB.Status = status;
                pedidoDB.Situacao = motivo;
                pedidoDB.ModifiedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(pedidoDB);

                return (200, true);

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Concat("Error: ", ex.Message), ex);
                return (500, false);
            }
        }
        private async Task GetProdutoAsync(SemaphoreSlim semaphore, Guid pedidoItemId, int QuantidadeSolicitada, ConcurrentQueue<PedidoItem> pedidoItems, CancellationTokenSource ctsToken)
        {
            await semaphore.WaitAsync(ctsToken.Token);

            try
            {
                var produto = await _produtoApi.GetProdutoByAsync(pedidoItemId).ConfigureAwait(false);

                if (produto.QuantidadeEstoque < QuantidadeSolicitada)
                {
                    ctsToken.Cancel();
                    _produtoApiValidation.Enqueue($"Quantidade para o {produto.Nome} é menor que a quantidade solicitada no pedido.");
                    return;
                }

                pedidoItems.Enqueue(new PedidoItem() { Id = pedidoItemId, Nome = produto.Nome, Preco = produto.Preco, Quantidade = QuantidadeSolicitada });
                return;
            }
            catch (ApiException ex)
            {
                ctsToken.Cancel();
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    _produtoApiValidation.Enqueue($"Produto não encotrado para o Id {pedidoItemId}.");
                    return;
                }

                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

    }
}

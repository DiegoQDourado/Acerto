using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Factories;
using PedidosApi.Domain.Models;
using PedidosApi.Domain.Services.Impl;
using PedidosApi.Infra.Configs;
using PedidosApi.Infra.Data.Repositories;
using PedidosApi.Infra.ExternalServices;
using PedidosApi.Infra.Models;
using Refit;
using System.Drawing;
using System.Net;
using Xunit;

namespace PedidosApi.Test.Domain.Services
{
    public class PedidoServiceTest
    {

        [Fact]
        public async Task AddAsync_PedidoNotFound_ReturnsBadRequest()
        {
            // Arrange
            var quantidade = 1;
            var preco = 1;
            var pedidoItem = new PedidoItem { Id = Guid.NewGuid(), Quantidade = quantidade };
            var pedido = new Pedido { PedidoItems = [pedidoItem], ValorTotal = preco };

            var pedidoRepository = new Mock<IPedidosRepository>();
            var pedidoFactory = new Mock<IPedidoFactory>();
            var loggerMock = new Mock<ILogger<PedidoService>>();

            var produtoApiConfig = new ProdutoApiConfig()
            {
                Protocol = "http",
                BaseUrl = "localhost",
                Port = "5000",
                ParallelCallCount = 5,
                Token = "token"
            };

            var produtoApi = Substitute.For<IProdutoApi>();
            var exception = await ApiException.Create(
                new HttpRequestMessage(),
                HttpMethod.Get,
                new HttpResponseMessage(HttpStatusCode.NotFound),
                new RefitSettings());
            produtoApi.GetProdutoByAsync(Arg.Any<Guid>()).Throws(exception);

            var pedidoService = new PedidoService(pedidoRepository.Object, pedidoFactory.Object, loggerMock.Object, produtoApiConfig, produtoApi);

            // Act
            (int statusCode, PedidoResponse? pedidoEntity) = await pedidoService.AddAsync(pedido);

            // Assert
            Assert.Equal(400, statusCode);
            Assert.Null(pedidoEntity);
            Assert.Single(pedidoService.ErrorsMessage);
            Assert.Equal($"Produto não encotrado para o Id {pedidoItem.Id}.", pedidoService.ErrorsMessage[0]);

        }

        [Fact]
        public async Task AddAsync_PedidoFieldValorTotalValidator_ReturnsBadRequest()
        {
            // Arrange
            const int quantidade = 1;
            const int preco = 1;
            const int valor = 0;

            var pedidoItem = new PedidoItem { Id = Guid.NewGuid(), Quantidade = quantidade };
            var pedido = new Pedido { PedidoItems = [pedidoItem], ValorTotal = valor };
            var produtoResponse = new ProdutoResponse() { Id = Guid.NewGuid(), Nome = "nome", Descricao = "descricao", Preco = preco, QuantidadeEstoque = quantidade };
            var pedidoId = Guid.NewGuid();
            var pedidoItemEntity = new PedidoItemEntity { Id = Guid.NewGuid(), ItemQuantidade = quantidade, ItemId = Guid.NewGuid(), ItemNome = "nome", ItemPreco = preco, PedidoId = pedidoId };
            var pedidoEntityResponse = new PedidoEntity()
            {
                Id = pedidoId,
                Codigo = 10,
                Descricao = "descricao",
                ValorTotal = valor,
                PedidoItems = [pedidoItemEntity]
            };

            var pedidoRepository = new Mock<IPedidosRepository>();
            var pedidoFactory = new Mock<IPedidoFactory>();
            var loggerMock = new Mock<ILogger<PedidoService>>();

            var produtoApiConfig = new ProdutoApiConfig()
            {
                Protocol = "http",
                BaseUrl = "localhost",
                Port = "5000",
                ParallelCallCount = 5,
                Token = "token"
            };

            pedidoFactory
                .Setup(x => x.From(It.IsAny<Pedido>()))
                .Returns(pedidoEntityResponse);

            var produtoApi = Substitute.For<IProdutoApi>();
            produtoApi.GetProdutoByAsync(Arg.Any<Guid>()).Returns(produtoResponse);

            var pedidoService = new PedidoService(pedidoRepository.Object, pedidoFactory.Object, loggerMock.Object, produtoApiConfig, produtoApi);

            // Act
            (int statusCode, PedidoResponse? pedidoEntity) = await pedidoService.AddAsync(pedido);

            // Assert
            Assert.Equal(400, statusCode);
            Assert.Null(pedidoEntity);
            Assert.Single(pedidoService.ErrorsMessage);
            Assert.Equal("Valor Total tem que ser maior que 0.", pedidoService.ErrorsMessage[0]);
        }

        [Fact]
        public async Task AddAsync_PedidoFieldValorTotalCompareSumPreco_ReturnsBadRequest()
        {
            // Arrange
            const int quantidade = 1;
            const int preco = 1;
            const int valor = 2;

            var pedidoItem = new PedidoItem { Id = Guid.NewGuid(), Quantidade = quantidade };
            var pedido = new Pedido { PedidoItems = [pedidoItem], ValorTotal = valor };
            var produtoResponse = new ProdutoResponse() { Id = Guid.NewGuid(), Nome = "nome", Descricao = "descricao", Preco = preco, QuantidadeEstoque = quantidade };
            var pedidoId = Guid.NewGuid();
            var pedidoItemEntity = new PedidoItemEntity { Id = Guid.NewGuid(), ItemQuantidade = quantidade, ItemId = Guid.NewGuid(), ItemNome = "nome", ItemPreco = preco, PedidoId = pedidoId };
            var pedidoEntityResponse = new PedidoEntity()
            {
                Id = pedidoId,
                Codigo = 10,
                Descricao = "descricao",
                ValorTotal = valor,
                PedidoItems = [pedidoItemEntity]
            };
            var pedidoItemResponse = new PedidoItemResponse { Id = Guid.NewGuid(), ItemQuantidade = quantidade, Nome = "nome", Preco = preco };
            var pedidoResponse = new PedidoResponse()
            {
                Id = pedidoId,
                Codigo = 10,
                Descricao = "descricao",
                ValorTotal = valor,
                PedidoItems = [pedidoItemResponse]
            };

            var pedidoRepository = new Mock<IPedidosRepository>();
            var pedidoFactory = new Mock<IPedidoFactory>();
            var loggerMock = new Mock<ILogger<PedidoService>>();

            var produtoApiConfig = new ProdutoApiConfig()
            {
                Protocol = "http",
                BaseUrl = "localhost",
                Port = "5000",
                ParallelCallCount = 5,
                Token = "token"
            };

            pedidoFactory
                .Setup(x => x.From(It.IsAny<Pedido>()))
                .Returns(pedidoEntityResponse);

            pedidoFactory
                .Setup(x => x.From(It.IsAny<PedidoEntity>()))
                .Returns(pedidoResponse);

            var produtoApi = Substitute.For<IProdutoApi>();
            produtoApi.GetProdutoByAsync(Arg.Any<Guid>()).Returns(produtoResponse);

            var pedidoService = new PedidoService(pedidoRepository.Object, pedidoFactory.Object, loggerMock.Object, produtoApiConfig, produtoApi);

            // Act
            (int statusCode, PedidoResponse? pedidoEntity) = await pedidoService.AddAsync(pedido);

            // Assert
            Assert.Equal(201, statusCode);
            Assert.NotNull(pedidoEntity);
            Assert.Single(pedidoService.ErrorsMessage);
            Assert.Equal("Valor Total diferente da soma dos preços para os produtos adcionados.", pedidoService.ErrorsMessage[0]);
        }

        [Fact]
        public async Task AddAsync_PedidoAdded_ReturnSuccess()
        {
            // Arrange
            var quantidade = 1;
            var preco = 1;
            var pedidoItem = new PedidoItem { Id = Guid.NewGuid(), Quantidade = quantidade };
            var pedido = new Pedido { PedidoItems = [pedidoItem], ValorTotal = preco };
            var produtoResponse = new ProdutoResponse() { Id = Guid.NewGuid(), Nome = "nome", Descricao = "descricao", Preco = preco, QuantidadeEstoque = quantidade };
            var pedidoId = Guid.NewGuid();
            var pedidoItemEntity = new PedidoItemEntity { Id = Guid.NewGuid(), ItemQuantidade = quantidade, ItemId = Guid.NewGuid(), ItemNome = "nome", ItemPreco = preco, PedidoId = pedidoId };
            var pedidoEntityResponse = new PedidoEntity()
            {
                Id = pedidoId,
                Codigo = 10,
                Descricao = "descricao",
                ValorTotal = preco,
                PedidoItems = [pedidoItemEntity]
            };
            var pedidoItemResponse = new PedidoItemResponse { Id = Guid.NewGuid(), ItemQuantidade = quantidade, Nome = "nome", Preco = preco };
            var pedidoResponse = new PedidoResponse()
            {
                Id = pedidoId,
                Codigo = 10,
                Descricao = "descricao",
                ValorTotal = preco,
                PedidoItems = [pedidoItemResponse]
            };

            var produtoRepository = new Mock<IPedidosRepository>();
            var pedidoFactory = new Mock<IPedidoFactory>();
            var loggerMock = new Mock<ILogger<PedidoService>>();

            produtoRepository.Setup(p => p.AddAsync(It.IsAny<PedidoEntity>()))
                .Returns(Task.FromResult(pedidoEntityResponse));

            pedidoFactory
                .Setup(x => x.From(It.IsAny<Pedido>()))
                .Returns(pedidoEntityResponse);
            
            pedidoFactory
                .Setup(x => x.From(It.IsAny<PedidoEntity>()))
                .Returns(pedidoResponse);

            var produtoApiConfig = new ProdutoApiConfig()
            {
                Protocol = "http",
                BaseUrl = "localhost",
                Port = "5000",
                ParallelCallCount = 5,
                Token = "token"
            };

            var produtoApi = Substitute.For<IProdutoApi>();
            produtoApi.GetProdutoByAsync(Arg.Any<Guid>()).Returns(produtoResponse);

            var pedidoService = new PedidoService(produtoRepository.Object, pedidoFactory.Object, loggerMock.Object, produtoApiConfig, produtoApi);

            // Act
            (int statusCode, PedidoResponse? pedidoEntity) = await pedidoService.AddAsync(pedido);

            // Assert
            Assert.Equal(201, statusCode);
            Assert.NotNull(pedidoEntity);
            Assert.Empty(pedidoService.ErrorsMessage);
        }
    }
}


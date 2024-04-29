using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Enums;
using PedidosApi.Domain.Models;

namespace PedidosApi.Domain.Services
{
    public interface IPedidoService
    {
        List<string> ErrorsMessage { get; }
        Task<(int StatusCode, PedidoResponse?)> AddAsync(Pedido value);
        Task<(int StatusCode, bool IsSuccess)> DeleteAsync(int codigo);
        Task<PedidoResponse?> GetByAsync(int codigo);
        Task<(int StatusCode, bool IsSuccess)> UpdateStatusAsync(int codigo, StatusPedido status, string motivo);
    }
}

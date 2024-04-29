using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.Application.Constants;
using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Extensions;
using PedidosApi.Domain.Models;
using PedidosApi.Domain.Services;

namespace PedidosApi.Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PedidosController(IPedidoService pedidoService) : ControllerBase
    {
        private readonly IPedidoService _pedidoService = pedidoService;

        [HttpGet("{codigo}")]
        [Authorize(Roles = Roles.GetPermission)]
        public async Task<IActionResult> Get(int codigo)
        {
            var pedido = await _pedidoService.GetByAsync(codigo);

            if (pedido is null) { return NotFound(); }

            return Ok(pedido);
        }

        [HttpPost]
        [Authorize(Roles = Roles.PostPermission)]
        public async Task<IActionResult> Post([FromBody] Pedido value)
        {
            (int statusCode, PedidoResponse? pedido) = await _pedidoService.AddAsync(value);

            if (pedido is null) { return StatusCode(statusCode, _pedidoService.ErrorsMessage.ToString(",")); }

            return Created($"/pedidos/{pedido.Id}", pedido);
        }

        [HttpDelete("{codigo}")]
        [Authorize(Roles = Roles.PostPermission)]
        public async Task<IActionResult> Delete(int codigo)
        {
            await _pedidoService.DeleteAsync(codigo);
            return Ok();
        }
    }
}

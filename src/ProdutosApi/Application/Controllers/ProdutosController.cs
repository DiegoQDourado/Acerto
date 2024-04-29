using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdutosApi.Application.Constants;
using ProdutosApi.Domain.Entities;
using ProdutosApi.Domain.Extensions;
using ProdutosApi.Domain.Models;
using ProdutosApi.Domain.Services;

namespace ProdutosApi.Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProdutosController(IProdutoService produtoService) : ControllerBase
    {
        private readonly IProdutoService _produtoService = produtoService;

        [Authorize(Roles = Roles.GetPermission)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var produto = await _produtoService.GetByAsync(id);

            if (produto == null) { return NotFound(); }

            return Ok(produto);
        }

        [Authorize(Roles = Roles.GetPermission)]
        [HttpGet("{id}/quantity")]
        public async Task<IActionResult> GetQuantidade(Guid id)
        {
            (int statusCode, bool isSuccess, int quantity) = await _produtoService.GetQuantidadeByAsync(id);

            if (!isSuccess) { return StatusCode(statusCode, _produtoService.ErrorsMessage.ToString(",")); }

            return Ok(quantity);
        }

        [Authorize(Roles = Roles.PostPermission)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto value)
        {
            (int statusCode, ProdutoEntity? produto) = await _produtoService.AddAsync(value);

            if (produto is null) { return StatusCode(statusCode, _produtoService.ErrorsMessage.ToString(",")); }

            return Created($"/produtos/{produto.Id}", value);
        }

        [Authorize(Roles = Roles.PutPermission)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Produto value)
        {
            (int statusCode, bool isSuccess) = await _produtoService.UpdateAsync(id, value);

            if (!isSuccess) { return StatusCode(statusCode, _produtoService.ErrorsMessage.ToString(",")); }

            return Ok();
        }

        [Authorize(Roles = Roles.DeletePermission)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            (int statusCode, bool isSuccess) = await _produtoService.DeleteAsync(id);

            if (!isSuccess) { return StatusCode(statusCode, _produtoService.ErrorsMessage.ToString(",")); }

            return Ok();
        }
    }
}

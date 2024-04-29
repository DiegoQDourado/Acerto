using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace AutenticationApi.Repositories.Impl
{
    internal class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;

            var produtoInfo = new AuthenticationInfo()
            {
                ApiKey = "produtoapi",
                SecretKey = _configuration.GetValue<string>("Jwt:SecretKey_Produto"),
                Claims =
                [
                    new(ClaimTypes.Role, "PRODUTO.POST"),
                    new(ClaimTypes.Role, "PRODUTO.PUT"),
                    new(ClaimTypes.Role, "PRODUTO.DELETE"),
                    new(ClaimTypes.Role, "PRODUTO.GET"),
                ]
            };
            _cache.Set("produtoapi", produtoInfo);

            var pedidoInfo = new AuthenticationInfo()
            {
                ApiKey = "pedidoapi",
                SecretKey = _configuration.GetValue<string>("Jwt:SecretKey_Pedido"),
                Claims =
                [
                    new(ClaimTypes.Role, "PEDIDO.POST"),
                    new(ClaimTypes.Role, "PEDIDO.GET"),
                ]
            };
            _cache.Set("pedidoapi", pedidoInfo);
        }

        public AuthenticationInfo GetBy(string apiKey) =>
            _cache.Get<AuthenticationInfo>(apiKey);
    }

    public record AuthenticationInfo
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public List<Claim> Claims { get; set; }
    }
}

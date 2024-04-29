using AutenticationApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutenticationApi.Services.Impl
{
    internal class AuthenticationService(IAuthenticationRepository repository) : IAuthenticationService
    {
        private readonly IAuthenticationRepository _repository = repository;

        public string? GenerateToken(string apiKey)
        {
            var authenticationInfo = _repository.GetBy(apiKey);
            if (authenticationInfo is null) return null;

            var key = Encoding.ASCII.GetBytes(authenticationInfo.SecretKey);
            authenticationInfo.Claims.Add(new(ClaimTypes.NameIdentifier, apiKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authenticationInfo.Claims),
                Expires = DateTime.MaxValue,// DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

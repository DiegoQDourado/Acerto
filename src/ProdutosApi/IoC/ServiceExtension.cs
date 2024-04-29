using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProdutosApi.Domain.Factories;
using ProdutosApi.Domain.Factories.Impl;
using ProdutosApi.Domain.Services;
using ProdutosApi.Domain.Services.Impl;
using ProdutosApi.Infra.Data;
using ProdutosApi.Infra.Data.Repositories;
using ProdutosApi.Infra.Data.Repositories.Impl;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ProdutosApi.IoC
{
    public static class ServiceExtension
    {
        public static IServiceCollection IoC(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddJwt(configuration)
                .AddInfra(configuration)
                .AddDomain();

        public static IServiceCollection AddDomain(this IServiceCollection services) =>
            services
                .AddScoped<IProdutoFactory, ProdutoFactory>()
                .AddScoped<IProdutoService, ProdutoService>();


        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDb(configuration)
                .AddScoped<IProdutosRepository, ProdutosRepository>();

        public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ProdutosConnectionString");
            return services.AddDbContext<ProdutosContext>(options => options.UseNpgsql(connectionString));
        }

        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration.GetValue<string>("Jwt:SecretKey");
            var validateSigningKey = !string.IsNullOrWhiteSpace(secretKey);

            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.RequireHttpsMetadata = false;
                    jwtOptions.SaveToken = true;
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = validateSigningKey,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                    };

                    if (validateSigningKey)
                    {
                        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                        jwtOptions.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes);
                        jwtOptions.TokenValidationParameters.RequireExpirationTime = false;
                    }
                    else
                    {
                        jwtOptions.TokenValidationParameters.RequireExpirationTime = false;
                        jwtOptions.TokenValidationParameters.RequireSignedTokens = false;
                        jwtOptions.TokenValidationParameters.SignatureValidator = (token, _) =>
                            new JwtSecurityToken(token);
                    }
                });

            return services;
        }

    }
}

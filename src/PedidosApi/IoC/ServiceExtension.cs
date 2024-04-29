using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PedidosApi.Domain.Factories;
using PedidosApi.Domain.Factories.Impl;
using PedidosApi.Domain.Services;
using PedidosApi.Domain.Services.Impl;
using PedidosApi.Infra.Configs;
using PedidosApi.Infra.Data;
using PedidosApi.Infra.Data.Repositories;
using PedidosApi.Infra.Data.Repositories.Impl;
using PedidosApi.Infra.ExternalServices;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PedidosApi.IoC
{
    public static class ServiceExtension
    {
        public static IServiceCollection IoC(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddJwt(configuration)
                .AddConfigs(configuration)
                .AddInfra(configuration)
                .AddDomain();

        public static IServiceCollection AddDomain(this IServiceCollection services) =>
            services
                .AddScoped<IPedidoItemFactory, PedidoItemFactory>()
                .AddScoped<IPedidoFactory, PedidoFactory>()
                .AddScoped<IPedidoService, PedidoService>();

        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDb(configuration)
                .AddScoped<IPedidosRepository, PedidosRepository>();

        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("ProdutoApi").Get<ProdutoApiConfig>());
            var url = $"{configuration["ProdutoApi:Protocol"]}://{configuration["ProdutoApi:BaseUrl"]}:{configuration["ProdutoApi:Port"]}";
            var settings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(configuration["ProdutoApi:Token"])
            };
            services
                .AddRefitClient<IProdutoApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(url));


            return services;
        }

        public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PedidosConnectionString");
            return services.AddDbContext<PedidosContext>(options => options.UseNpgsql(connectionString));
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

using Microsoft.OpenApi.Models;
using PedidosApi.Infra.ConsumerMessage;
using PedidosApi.IoC;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting(op => op.LowercaseUrls = true)
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.IoC(builder.Configuration);
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = @"Inserir JWT com a informação de Bearer no campo; Exemplo: ""Bearer &lt;TOKEN&gt;"".",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            Array.Empty<string>()
                        },
                    });
});
builder.Services.AddHostedService<Consumer>();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();
using AutenticationApi.Repositories;
using AutenticationApi.Repositories.Impl;
using AutenticationApi.Services;
using AutenticationApi.Services.Impl;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/token", (string apiKey, IAuthenticationService autentication) =>
{
    var token = autentication.GenerateToken(apiKey.ToLower());

    if (string.IsNullOrEmpty(token))
        return Results.NotFound();

    return Results.Ok(token);
});

app.UseSwagger();
app.UseSwaggerUI();
app.Run();


namespace AutenticationApi.Services
{
    public interface IAuthenticationService
    {
        string? GenerateToken(string apiKey);
    }
}

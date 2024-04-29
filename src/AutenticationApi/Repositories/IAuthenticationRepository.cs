using AutenticationApi.Repositories.Impl;

namespace AutenticationApi.Repositories
{
    public interface IAuthenticationRepository
    {
        AuthenticationInfo GetBy(string apiKey);
    }
}

using IdentityModel.Client;

namespace Web.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GetTokenAsync(params string[] args);
    }
}

using IdentityModel.Client;

namespace Web.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(HttpContext context);
    }
}

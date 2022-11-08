using Api.Services.Gateway.Identity;
using System.Threading.Tasks;

namespace Desktop.App.Services.Authentication.TokenStorage
{
    public interface ITokenStorage
    {
        Task<TokenResponse?> GetTokenAsync();

        Task SaveTokenAsync(TokenResponse userData);

        Task RemoveTokenAsync();
    }
}
